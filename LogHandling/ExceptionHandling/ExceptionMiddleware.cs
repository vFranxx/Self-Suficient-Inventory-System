using Microsoft.AspNetCore.Http;
using Middleware.Models;
using Self_Suficient_Inventory_System.Data;
using System.Net;

namespace Self_Suficient_Inventory_System.LogHandling.ExceptionHandling
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AppDbContext dbContext)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionAsync(ex, context, dbContext);
            }

            await ExceptionHandler.LogClientErrorAsync(context, dbContext);
        }

    }
}