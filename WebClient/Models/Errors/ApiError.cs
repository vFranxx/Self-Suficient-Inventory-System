using System.Text.Json.Serialization;

namespace WebClient.Models.Errors
{
    public class ApiError
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
