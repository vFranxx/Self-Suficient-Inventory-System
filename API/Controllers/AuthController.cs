using API.Data;
using API.Models;
using API.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.DTO;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<SystemOperator> _userManager;
        private readonly SignInManager<SystemOperator> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;

        public AuthController(UserManager<SystemOperator> userManager, SignInManager<SystemOperator> signInManager, IConfiguration configuration, AppDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginModelDTO model)
        {
            // Validate input
            if (string.IsNullOrEmpty(model.User) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new { message = "Username/email and password are required" });
            }

            SystemOperator user = null;
            if (model.User.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(model.User);
            }
            else
            {
                user = await _userManager.FindByNameAsync(model.User);
            }

            //if (!user.EmailConfirmed) 
            //{
            //    return BadRequest("Es necesario verificar la dirección de correo");
            //}

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }
            
            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Generate JWT token
            var (jwtAccessToken, jti) = await GenerateJwtToken(user, roles);

            // Generate refresh token
            var refreshToken = await GenerateRefreshToken(jti, user.Id);

            var response = new JwtTokenResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtAccessToken),
                Expiration = jwtAccessToken.ValidTo,
                RefreshToken = refreshToken.Token,
                Roles = roles.ToList()
            };

            return Ok(response);   
        }

        private async Task<(JwtSecurityToken token, string jti)> GenerateJwtToken(SystemOperator user, IList<string> roles)
        {
            var jti = Guid.NewGuid().ToString();

            // Create claims
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id), // Required for Identity Integration
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, jti)
                };

            // Add roles to claim
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Configure token
            var secretKey = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("La clave JWT no está configurada");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create token
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(45),
                signingCredentials: creds
            );

            return (token, jti);
        }

        private async Task<UserRefreshJwtToken> GenerateRefreshToken(string jwtId, string userId)
        {
            var refreshToken = new UserRefreshJwtToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                JwtId = jwtId,
                UserId = userId,
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(7),
                Used = false,
                Invalidated = false
            };

            _dbContext.UserRefreshJwtTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync();

            return refreshToken;
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(RenewTokenRequestDTO dto)
        {
            var result = await RenewTokenAsync(dto);

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.response);
        }

        private async Task<(string ErrorMessage, JwtTokenResponseDTO response)> RenewTokenAsync(RenewTokenRequestDTO dto)
        {
            var existingRefreshToken = await _dbContext.UserRefreshJwtTokens
                .Where(_ => _.UserId == dto.UserId &&
                       _.Token == dto.RefreshToken &&
                       !_.Used &&
                       !_.Invalidated &&
                       _.ExpirationDate > DateTime.UtcNow).FirstOrDefaultAsync();

            if (existingRefreshToken == null)
            {
                return ("Invalid or expired refresh token", null);
            }

            // Marcar token como usado
            existingRefreshToken.Used = true;
            _dbContext.UserRefreshJwtTokens.Update(existingRefreshToken);

            // Obtener usuario
            var user = await _dbContext.SystemOperators.FindAsync(dto.UserId);
            if (user == null)
                return ("User not found", null);

            // Obtener roles
            var roles = await _userManager.GetRolesAsync(user);

            var (newJwtAccessToken, newJti) = await GenerateJwtToken(user, roles);
            var newRefreshToken = await GenerateRefreshToken(newJti, user.Id);

            await _dbContext.SaveChangesAsync();

            var response = new JwtTokenResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(newJwtAccessToken),
                Expiration = newJwtAccessToken.ValidTo,
                RefreshToken = newRefreshToken.Token,
                Roles = roles.ToList()
            };

            return (null, response);
        }

        [HttpPost("logout")]
        [Authorize]  // Requiere autenticación
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Invalidar todos los tokens del usuario
            var tokens = await _dbContext.UserRefreshJwtTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.Invalidated = true;
            }

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
