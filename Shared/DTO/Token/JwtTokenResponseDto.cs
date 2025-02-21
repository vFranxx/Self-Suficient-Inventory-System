namespace Shared.DTO.Token
{
    public class JwtTokenResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public List<string> Roles { get; set; }
        public string RefreshToken { get; set; }
    }
}
