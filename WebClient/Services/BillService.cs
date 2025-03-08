using WebClient.Models.Entities;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace WebClient.Services
{
    public class BillService
    {
        private readonly HttpClient _httpClient;

        public BillService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Bill> CreateBillAsync(Bill bill)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Bill", bill);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Bill>() ?? throw new Exception("Error al crear la factura");
            }
            throw new Exception("No se pudo crear la factura");
        }

        public async Task<bool> CreateBillDetailAsync(BillDetail billDetail)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/BillDetail/bill/{billDetail.IdFactura}/details", new List<BillDetail> { billDetail });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProductStockAsync(string productId, int cantidadVendida)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Product/remove-from-stock/{productId}", new { stock = cantidadVendida });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ProcessSaleAsync(List<CartItem> cartItems, string operadorId)
        {
            try
            {
                decimal total = cartItems.Sum(i => i.Quantity * i.Product.PrecioUnitario);
                var bill = new Bill
                {
                    FechaHora = DateTime.Now,
                    Total = total,
                    IdOp = operadorId
                };

                var createdBill = await CreateBillAsync(bill);
                if (createdBill == null) return false;

                foreach (var item in cartItems)
                {
                    var billDetail = new BillDetail
                    {
                        IdFactura = createdBill.FacId,
                        IdProducto = item.Product.ProdId,
                        Cantidad = item.Quantity,
                        Precio = item.Product.PrecioUnitario,
                        Subtotal = item.Quantity * item.Product.PrecioUnitario
                    };

                    bool detailAdded = await CreateBillDetailAsync(billDetail);
                    if (!detailAdded) throw new Exception("Error al guardar detalles de la factura");

                    bool stockUpdated = await UpdateProductStockAsync(item.Product.ProdId, item.Quantity);
                    if (!stockUpdated) throw new Exception("Error al actualizar el stock del producto");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en ProcessSaleAsync: {ex.Message}");
                return false;
            }
        }
    }

    public class CartItem
    {
        public required Product Product { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
