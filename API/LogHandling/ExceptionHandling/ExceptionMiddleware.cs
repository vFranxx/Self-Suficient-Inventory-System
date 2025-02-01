using API.Data;

namespace API.LogHandling.ExceptionHandling
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