using RESTful_API.Models.Entities;
using Middleware.Models;
using RESTful_API.Data;
using System.Net;

namespace Self_Suficient_Inventory_System.LogHandling.ResponseHandle
{
    public static class ResponseHandler
    {
        public static async Task<int> HandleResponseAsync(HttpContext context, AppDbContext dbContext, bool writeResponse = true)
        {
            return -1;
        }
    }
}
