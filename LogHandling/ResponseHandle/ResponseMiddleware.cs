using Middleware;
using RESTful_API.Data;

namespace Self_Suficient_Inventory_System.LogHandling.ResponseHandle
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AppDbContext dbContext)
        {
        }
    }
}
