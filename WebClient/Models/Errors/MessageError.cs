using System.Text.Json.Serialization;

namespace WebClient.Models.Errors
{
    public class MessageError
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
