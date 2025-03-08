using System.Net.Http.Json;
using System.Threading.Tasks;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var requestBody = new { email, password };
        var response = await _httpClient.PostAsJsonAsync("api/Auth/login", requestBody);

        if (response.IsSuccessStatusCode)
        {
            return true;  // Usuario autenticado correctamente
        }
        else
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"❌ Error en el login: {errorMessage}");
            return false;
        }
    }
}
