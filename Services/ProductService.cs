<<<<<<< HEAD
﻿using BlazorFront.Models.Entities;
using System.Net.Http.Json;

namespace BlazorFront.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7047/api/");
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                Console.WriteLine("📡 Solicitando lista de productos...");
                var response = await _httpClient.GetAsync("Product/all");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Error al obtener productos: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return new List<Product>();
                }

                var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                Console.WriteLine("✅ Productos obtenidos correctamente.");
                return products ?? new List<Product>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inesperado al obtener productos: {ex.Message}");
                return new List<Product>();
            }
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            try
            {
                Console.WriteLine($"📤 Enviando producto: {System.Text.Json.JsonSerializer.Serialize(product)}");

                var response = await _httpClient.PostAsJsonAsync("Product", product);
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"🔄 Respuesta de la API: {response.StatusCode} - {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inesperado al agregar producto: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(string prodId)
        {
            try
            {
                Console.WriteLine($"🗑 Eliminando producto ID: {prodId}");
                var response = await _httpClient.DeleteAsync($"Product/{prodId}");
                Console.WriteLine($"🔄 Respuesta API: {response.StatusCode}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inesperado al eliminar producto: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                Console.WriteLine($"🔄 Actualizando producto ID: {product.ProdId}");
                var response = await _httpClient.PutAsJsonAsync($"Product/{product.ProdId}", product);

                Console.WriteLine($"🔄 Respuesta API: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inesperado al actualizar producto: {ex.Message}");
                return false;
            }
        }
    }
}
=======
﻿using BlazorFront.Models.Entities;
using System.Net.Http.Json;

namespace BlazorFront.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7047/api/");
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                Console.WriteLine("📡 Solicitando lista de productos...");
                var response = await _httpClient.GetAsync("Product/all");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Error al obtener productos: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return new List<Product>();
                }

                var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                Console.WriteLine("✅ Productos obtenidos correctamente.");
                return products ?? new List<Product>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inesperado al obtener productos: {ex.Message}");
                return new List<Product>();
            }
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            try
            {
                Console.WriteLine($"📤 Enviando producto: {System.Text.Json.JsonSerializer.Serialize(product)}");

                var response = await _httpClient.PostAsJsonAsync("Product", product);
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"🔄 Respuesta de la API: {response.StatusCode} - {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inesperado al agregar producto: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(string prodId)
        {
            try
            {
                Console.WriteLine($"🗑 Eliminando producto ID: {prodId}");
                var response = await _httpClient.DeleteAsync($"Product/{prodId}");
                Console.WriteLine($"🔄 Respuesta API: {response.StatusCode}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inesperado al eliminar producto: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                Console.WriteLine($"🔄 Actualizando producto ID: {product.ProdId}");
                var response = await _httpClient.PutAsJsonAsync($"Product/{product.ProdId}", product);

                Console.WriteLine($"🔄 Respuesta API: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inesperado al actualizar producto: {ex.Message}");
                return false;
            }
        }
    }
}
>>>>>>> bd9cc5e72f2d3a84ac6a39a51b72b57afbf7aae1
