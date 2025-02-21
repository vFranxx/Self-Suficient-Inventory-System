using API.Data;
using API.Models;
using API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.DTO.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Services.Token
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;
        private readonly UserManager<SystemOperator> _userManager;

        public JwtService(
            IConfiguration configuration,
            AppDbContext dbContext,
            UserManager<SystemOperator> userManager)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<(JwtSecurityToken token, string jti)> GenerateJwtToken(SystemOperator user, IList<string> roles)
        {
            var jti = Guid.NewGuid().ToString();

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            new Claim(JwtRegisteredClaimNames.Jti, jti)
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var secretKey = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
                throw new InvalidOperationException("La clave JWT no está configurada");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return (new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(45),
                signingCredentials: creds
            ), jti);
        }

        public async Task<UserRefreshJwtToken> GenerateRefreshToken(string jwtId, string userId)
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

        public async Task<(string ErrorMessage, JwtTokenResponseDto response)> RenewTokenAsync(RenewTokenRequestDTO dto)
        {
            var existingRefreshToken = await _dbContext.UserRefreshJwtTokens
                .FirstOrDefaultAsync(t =>
                    t.UserId == dto.UserId &&
                    t.Token == dto.RefreshToken &&
                    !t.Used &&
                    !t.Invalidated &&
                    t.ExpirationDate > DateTime.UtcNow);

            if (existingRefreshToken == null)
                return ("Invalid or expired refresh token", null);

            existingRefreshToken.Used = true;
            _dbContext.UserRefreshJwtTokens.Update(existingRefreshToken);

            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return ("User not found", null);

            var roles = await _userManager.GetRolesAsync(user);
            var (newJwtAccessToken, newJti) = await GenerateJwtToken(user, roles);
            var newRefreshToken = await GenerateRefreshToken(newJti, user.Id);

            await _dbContext.SaveChangesAsync();

            return ("", new JwtTokenResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(newJwtAccessToken),
                Expiration = newJwtAccessToken.ValidTo,
                RefreshToken = newRefreshToken.Token,
                Roles = roles.ToList()
            });
        }

        public async Task<int> InvalidateUserTokensAsync(string userId)
        {
            var activeTokens = await _dbContext.UserRefreshJwtTokens
                .Where(t => t.UserId == userId &&
                       !t.Invalidated &&
                       t.ExpirationDate > DateTime.UtcNow)
                .ToListAsync();

            if (!activeTokens.Any()) return 0;

            foreach (var token in activeTokens)
            {
                token.Invalidated = true;
            }
            return await _dbContext.SaveChangesAsync();
        }
    }
}