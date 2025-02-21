using API.Models.Entities;
using API.Models;
using System.IdentityModel.Tokens.Jwt;
using Shared.DTO.Token;

namespace API.Services.Token
{
    public interface IJwtService
    {
        Task<(JwtSecurityToken token, string jti)> GenerateJwtToken(SystemOperator user, IList<string> roles);
        Task<UserRefreshJwtToken> GenerateRefreshToken(string jwtId, string userId);
        Task<(string ErrorMessage, JwtTokenResponseDto response)> RenewTokenAsync(RenewTokenRequestDTO dto);
        Task<int> InvalidateUserTokensAsync(string userId);
    }
}
