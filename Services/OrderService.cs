using System.Net.Http.Json;
using BlazorFront.Models.Entities;

namespace BlazorFront.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Order>>("api/Order") ?? new();
        }

        public async Task<List<OrderDetail>> GetOrderDetailsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<OrderDetail>>("api/OrderDetail") ?? new();
        }

        public async Task<Order?> CreateOrderAsync(Order order)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Order", order);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Order>() : null;
        }

        public async Task<bool> AddOrderDetailAsync(OrderDetail orderDetail)
        {
            var response = await _httpClient.PostAsJsonAsync("api/OrderDetail", orderDetail);
            return response.IsSuccessStatusCode;
        }
    }
}
