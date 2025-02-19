using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.DTO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace WebClient.Shared.Providers
{
    public class CustomHttpHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _http;
        private readonly AuthenticationStateProvider _auth;

        public CustomHttpHandler(ILocalStorageService localStorage, IHttpClientFactory http, AuthenticationStateProvider auth)
        {
            _localStorage = localStorage;
            _http = http;
            _auth = auth;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (SkipAuth(request)) 
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var token = await _localStorage.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var originalResponse = await base.SendAsync(request, cancellationToken);
            if (originalResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                return await RefreshTokenAsync(request, originalResponse, token, cancellationToken);
            }

            return originalResponse;
        }

        private bool SkipAuth(HttpRequestMessage request)
        {
            var path = request.RequestUri.AbsolutePath.ToLower();
            return path.Contains("login") || path.Contains("register") || path.Contains("refresh-token");
        }

        private async Task<HttpResponseMessage> RefreshTokenAsync(HttpRequestMessage originalRequest, 
            HttpResponseMessage originalResponse ,string jwtToken, CancellationToken cancellationToken)
        {
            // Refresh token
            var refreshToken = await _localStorage.GetItemAsync<string>("refresh-token");

            var userClaims = Utilities.ParseClaimsFromJwt(jwtToken);

            // Crear el DTO para enviar
            var refreshTokenRequest = new RenewTokenRequestDTO
            {
                RefreshToken = refreshToken,
                UserId = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            };

            var httpClient = _http.CreateClient("API");
            var response = await httpClient.PostAsJsonAsync("api/Auth/refresh-token", refreshTokenRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JwtTokenResponseDTO>();
                if (result != null) 
                {
                    await _localStorage.SetItemAsync("token", result.Token);
                    await _localStorage.SetItemAsync("refresh-token", result.RefreshToken);
                    await _localStorage.SetItemAsync("token-expiration", result.Expiration);
                    (_auth as CustomAuthProvider).NotifyAuthState();
                    
                    originalRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);

                    return await base.SendAsync(originalRequest, cancellationToken);
                }
            }

            return originalResponse;
        }
    }
}
