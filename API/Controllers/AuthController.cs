using API.Data;
using API.Models;
using API.Models.Entities;
using API.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.DTO;
using Shared.DTO.Token;
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
        private readonly IJwtService _jwtService;

        public AuthController(UserManager<SystemOperator> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginModelDto model)
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
            var (jwtAccessToken, jti) = await _jwtService.GenerateJwtToken(user, roles);

            // Generate refresh token
            var refreshToken = await _jwtService.GenerateRefreshToken(jti, user.Id);

            var response = new JwtTokenResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtAccessToken),
                Expiration = jwtAccessToken.ValidTo,
                RefreshToken = refreshToken.Token,
                Roles = roles.ToList()
            };

            return Ok(response);   
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(RenewTokenRequestDTO dto)
        {
            var result = await _jwtService.RenewTokenAsync(dto);

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.response);
        }

        [HttpPost("logout")]
        [Authorize]  // Requiere autenticación
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Usuario no autenticado" });

            var sessionsClosed = await _jwtService.InvalidateUserTokensAsync(userId);

            return Ok(new
            {
                Message = "Logout exitoso",
                SessionsClosed = sessionsClosed
            });
        }
    }
}
