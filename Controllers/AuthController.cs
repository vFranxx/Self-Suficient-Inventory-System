using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RESTful_API.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Self_Suficient_Inventory_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<SystemOperator> _userManager;
        private readonly SignInManager<SystemOperator> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<SystemOperator> userManager, SignInManager<SystemOperator> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> GenerateToken(LoginModel model)
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

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // Get user roles
                var roles = await _userManager.GetRolesAsync(user);

                // Create claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id), // Required for Identity Integration
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    roles
                });
            }

            return Unauthorized(new { message = "Credenciales inválidas" });
        }

        public class LoginModel
        {
            public string User { get; set; } // Toma mail o username
            public string Password { get; set; }
        }
    }
}
