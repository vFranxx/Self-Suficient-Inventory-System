using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using RESTful_API.Models.Entities;
using BlazorFront.Models.Entities;

public class InventoryService
{
    private readonly HttpClient _httpClient;

    public InventoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        var response = await _httpClient.GetAsync("api/products");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Product>>(json);
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"api/products/{id}");
        return response.IsSuccessStatusCode;
    }
}