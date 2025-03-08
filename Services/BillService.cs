<<<<<<< HEAD
﻿using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorFront.Models.Entities;
using Microsoft.JSInterop;

namespace BlazorFront.Services
{
    public class BillService
    {
        private readonly HttpClient _httpClient;

        public BillService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Bill?> CreateBillAsync(Bill bill)
        {
            try
            {
                Console.WriteLine($"📤 Enviando Factura: {System.Text.Json.JsonSerializer.Serialize(bill)}");

                var response = await _httpClient.PostAsJsonAsync("api/Bill", bill);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error API: Código {response.StatusCode} - {error}");
                    return null;
                }

                var createdBill = await response.Content.ReadFromJsonAsync<Bill>();
                Console.WriteLine($"✅ Factura creada con ID: {createdBill?.FacId}");
                return createdBill;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Excepción: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> RegistrarVenta(Bill nuevaFactura)
        {
            var response = await _httpClient.PostAsJsonAsync("api/bills", nuevaFactura);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Error API: {error}");
                return false;
            }

            return true;
        }

        public async Task<bool> CreateBillDetailAsync(BillDetail billDetail)
        {
            var response = await _httpClient.PostAsJsonAsync("api/billdetails", billDetail);
            return response.IsSuccessStatusCode;
        }
    }
=======
﻿using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorFront.Models.Entities;
using Microsoft.JSInterop;

namespace BlazorFront.Services
{
    public class BillService
    {
        private readonly HttpClient _httpClient;

        public BillService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Bill?> CreateBillAsync(Bill bill)
        {
            try
            {
                Console.WriteLine($"📤 Enviando Factura: {System.Text.Json.JsonSerializer.Serialize(bill)}");

                var response = await _httpClient.PostAsJsonAsync("api/Bill", bill);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error API: Código {response.StatusCode} - {error}");
                    return null;
                }

                var createdBill = await response.Content.ReadFromJsonAsync<Bill>();
                Console.WriteLine($"✅ Factura creada con ID: {createdBill?.FacId}");
                return createdBill;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Excepción: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> RegistrarVenta(Bill nuevaFactura)
        {
            var response = await _httpClient.PostAsJsonAsync("api/bills", nuevaFactura);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Error API: {error}");
                return false;
            }

            return true;
        }

        public async Task<bool> CreateBillDetailAsync(BillDetail billDetail)
        {
            var response = await _httpClient.PostAsJsonAsync("api/billdetails", billDetail);
            return response.IsSuccessStatusCode;
        }
    }
>>>>>>> bd9cc5e72f2d3a84ac6a39a51b72b57afbf7aae1
}