using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using BlazorFront.Models.Entities;

namespace BlazorFront.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Product>>("api/Product/all");
                return response ?? new List<Product>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener productos: {ex.Message}");
                return new List<Product>();
            }
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Product", product);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al agregar producto: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteProductAsync(string prodId)
        {
            try
            {
                if (int.TryParse(prodId, out int prodIdInt))
                {
                    var response = await _httpClient.DeleteAsync($"api/Product/{prodIdInt}");
                    return response.IsSuccessStatusCode;
                }
                else
                {
                    Console.WriteLine($"Error: ID de producto inválido.");
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al eliminar producto: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/Product", product);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al actualizar producto: {ex.Message}");
                return false;
            }
        }
    }
}
