using RESTful_API.Models.Entities;
using Middleware.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Self_Suficient_Inventory_System.Models.LogModels;
using Newtonsoft.Json;
using Self_Suficient_Inventory_System.Data;

namespace Self_Suficient_Inventory_System.LogHandling.ResponseHandle
{
    public static class ResponseHandler
    {
        /// <summary>
        /// Maneja el registro de la respuesta en la base de datos.
        /// </summary>
        /// <param name="context">El contexto HTTP de la solicitud.</param>
        /// <param name="newBodyStream">El cuerpo de la respuesta para leerla.</param>
        /// <returns>El ID del log de la respuesta.</returns>
        public static async Task<int> HandleResponseAsync(HttpContext context, AppDbContext _dbContext, MemoryStream newBodyStream)
        {
            // Leer la respuesta
            newBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(newBodyStream).ReadToEndAsync();

            // Detalles de la respuesta
            var responseLogEntry = new ResponseLogEntry
            {
                Timestamp = DateTime.Now,
                RequestUrl = context.Request.Path,
                HttpMethod = context.Request.Method,
                StatusCode = context.Response.StatusCode,
                ResponseBody = responseBody,
                Headers = JsonConvert.SerializeObject(context.Response.Headers),
                UserId = context.User?.Identity?.Name,  // Re-ver cuando vea autenticacion
                Origin = "API"
            };

            _dbContext.ResponseLogEntries.Add(responseLogEntry);
            await _dbContext.SaveChangesAsync();

            return responseLogEntry.Id;
        }
    }
}
