using BlazorFront;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorFront.Services;
using System.Net.Http;
using MudBlazor.Services;
using static System.Net.WebRequestMethods;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7047/api/") });

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<BillService>();
builder.Services.AddScoped<SupplierService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<InventoryService>();

await builder.Build().RunAsync();
