namespace Shared.DTO
{
    public class JwtTokenResponseDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public List<string> Roles { get; set; }
        public string RefreshToken { get; set; }
    }
}
