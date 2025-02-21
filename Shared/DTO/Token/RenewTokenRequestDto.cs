namespace Shared.DTO.Token
{
    public class RenewTokenRequestDTO
    {
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
