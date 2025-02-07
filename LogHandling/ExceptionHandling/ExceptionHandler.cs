using Middleware.Models;
using Self_Suficient_Inventory_System.API.LogHandling.ExceptionHandling;
using Self_Suficient_Inventory_System.Data;
using Self_Suficient_Inventory_System.Models.LogModels;
using System.Net;

namespace Self_Suficient_Inventory_System.LogHandling.ExceptionHandling
{
    public static class ExceptionHandler
    {
        /// <summary>
        /// Maneja una excepcion logueando los detalles del mismo en la base de datos.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="context"></param>
        /// <param name="dbContext"></param>
        /// <param name="writeResponse">Indica si el manejador de excepcion se encargue de escribir la respuesta de la API.
        /// Dejar en falso si se desea evitar que se sobreescriba la respuesta</param>
        /// <returns></returns>
        public static async Task<int> HandleExceptionAsync(Exception ex, HttpContext context, AppDbContext dbContext, bool writeResponse = true)
        {
            if (!context.Response.HasStarted)
            {
                var model = new UnhandledExceptionModel(ex);
                var response = model.ErrorMessage;

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if (writeResponse)
                {
                    await context.Response.WriteAsJsonAsync(response).ConfigureAwait(false);
                }

                var logEntry = new ExceptionLogEntry
                {
                    RequestUrl = context.Request.Path,
                    StatusCode = context.Response.StatusCode,
                    Timestamp = DateTime.Now,
                    ExceptionType = ex.GetType().ToString(),
                    ExceptionMessage = model.ErrorMessage,
                    StackTrace = model.StackTrace,
                    Origin = "API",
                };

                dbContext.ExceptionLogEntries.Add(logEntry);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);

                return logEntry.Id;
            }
            else
            {
                throw ex;
            }
        }


        /// <summary>
        /// Maneja errores que no necesariamente provengan de una excepcion.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static async Task<int> LogClientErrorAsync(HttpContext context, AppDbContext dbContext)
        {
            if (context.Response.StatusCode >= 400 && context.Response.StatusCode < 500)
            {
                var logEntry = new ExceptionLogEntry
                {
                    RequestUrl = context.Request.Path,
                    StatusCode = context.Response.StatusCode,
                    Timestamp = DateTime.Now,
                    Origin = "API",
                };

                dbContext.ExceptionLogEntries.Add(logEntry);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);

                return logEntry.Id;
            }

            return -1;
        }
    }

}
