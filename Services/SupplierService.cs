using BlazorFront.Models.Entities;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace BlazorFront.Services
{
    public class SupplierService
    {
        private readonly HttpClient _httpClient;

        public SupplierService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Supplier>> GetSuppliersAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Supplier>>("Supplier") ?? new List<Supplier>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener proveedores: {ex.Message}");
                return new List<Supplier>();
            }
        }

        public async Task<bool> AddSupplierAsync(Supplier supplier)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Supplier", supplier);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al agregar proveedor: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddSupplierProductAsync(SupplierProduct supplierProduct)
        {
            var response = await _httpClient.PostAsJsonAsync("api/SupplierProduct", supplierProduct);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<SupplierProduct>> GetAllSupplierProductsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<SupplierProduct>>("api/SupplierProduct") ?? new List<SupplierProduct>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener productos de proveedores: {ex.Message}");
                return new List<SupplierProduct>();
            }
        }

        public async Task<bool> UpdateSupplierAsync(Supplier supplier)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"Supplier/{supplier.ProvId}", supplier);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar proveedor: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteSupplierAsync(int provId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Supplier/{provId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar proveedor: {ex.Message}");
                return false;
            }
        }
    }
}
