using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebClient.Components
{
    public static class HandleApiError
    {
        public static async Task<string> GetErrorMessage(HttpResponseMessage response)
        {
            return response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => await ParseErrorResponse(response),
                HttpStatusCode.Forbidden => "No tienes permisos para realizar esta acción",
                HttpStatusCode.NotFound => await ParseStringResponse(response) ?? "No se encontró el dato",
                HttpStatusCode.BadRequest => await ParseErrorResponse(response),
                _ => $"Error desconocido - Código: {(int)response.StatusCode}"
            };
        }

        private static async Task<string?> ParseStringResponse(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                return !string.IsNullOrWhiteSpace(content)
                    ? content.Trim('"')
                    : null;
            }
            catch
            {
                return null;
            }
        }

        private static async Task<string> ParseErrorResponse(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(content))
                    return "Error desconocido";

                // Intentar deserializar como ApiError (objeto único)
                var error = JsonSerializer.Deserialize<ApiError>(content);
                if (error != null)
                {
                    if (!string.IsNullOrEmpty(error.Message))
                        return error.Message;

                    if (!string.IsNullOrEmpty(error.Description))
                        return error.Description;
                }

                // Intentar deserializar como lista de errores
                var errors = JsonSerializer.Deserialize<List<ApiError>>(content);
                if (errors?.Count > 0)
                    return string.Join(", ", errors.Select(e => !string.IsNullOrEmpty(e.Message) ? e.Message : e.Description));

                // Si todo falla, devolver el contenido crudo
                return content.Trim('"');
            }
            catch
            {
                return "Error en la solicitud";
            }
        }

        private class ApiError
        {
            [JsonPropertyName("code")]
            public string Code { get; set; }

            [JsonPropertyName("description")]
            public string Description { get; set; }

            [JsonPropertyName("message")]
            public string Message { get; set; }
        }
    }
}
