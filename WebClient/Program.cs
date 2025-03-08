using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebClient;
using MudBlazor.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WebClient.Shared.Providers;
using Blazored.Toast;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7026") });

builder.Services.AddHttpClient("API", options =>
{
    options.BaseAddress = new Uri("https://localhost:7026");
}).AddHttpMessageHandler<CustomHttpHandler>();

builder.Services.AddMudServices();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddBlazoredToast();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CustomHttpHandler>();

//Agrego los servicios
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<BillService>();
builder.Services.AddScoped<SupplierService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();
