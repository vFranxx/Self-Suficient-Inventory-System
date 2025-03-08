using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;  // Asegúrate de incluir esta directiva
using System.Threading.Tasks;
using Shared.DTO.Token;

public class AuthenticationService
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;

    public AuthenticationService(ILocalStorageService localStorage, HttpClient httpClient)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
    }

    // Método para renovar el JWT
    public async Task<string> RenewTokenAsync()
    {
        var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");
        var response = await _httpClient.PostAsJsonAsync("api/Auth/renew", new { RefreshToken = refreshToken });

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<JwtTokenResponseDto>(); // Reemplaza ReadAsAsync por ReadFromJsonAsync
            await _localStorage.SetItemAsync("authToken", data.Token);
            await _localStorage.SetItemAsync("refreshToken", data.RefreshToken);
            return data.Token;
        }
        return null;  // Si no se puede renovar el token, manejar el error aquí
    }

    // Método para obtener el token de autenticación
    public async Task<string> GetAuthTokenAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (string.IsNullOrEmpty(token))
        {
            // Aquí puedes manejar el caso de que no haya token y necesites hacer login
            return null;
        }
        return token;
    }

    // Método para hacer solicitudes HTTP autenticadas
    public async Task<HttpResponseMessage> MakeAuthenticatedRequestAsync(string url)
    {
        var token = await GetAuthTokenAsync();
        if (token != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetAsync(url);
        }
        return null;
    }
}