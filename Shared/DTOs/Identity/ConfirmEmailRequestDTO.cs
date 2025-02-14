namespace API.Shared.DTOs.Identity
{
    public class ConfirmEmailRequestDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
