using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using Middleware;
using RESTful_API.Data;
using System;

namespace Self_Suficient_Inventory_System.LogHandling.ResponseHandle
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext _dbContext)
        {
            var originalBodyStream = context.Response.Body;
            
            var excludedEndpoints = new HashSet<string>()
            {
                "/api/Test/",
                "/register",
                "/login",
                "/refresh",
                "/confirmEmail",
                "/resendConfirmationEmail",
                "/forgotPassword",
                "/resetPassword",
                "/manage/2fa",
                "/manage/info",
                "/manage/info",
                "/api/Auth/",
                "/api/Roles/"
            };

            if (excludedEndpoints.Any(endpoint => context.Request.Path.Value.StartsWith(endpoint, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }            

            try
            {
                using var newBodyStream = new MemoryStream();
                context.Response.Body = newBodyStream;

                await _next(context);
                await ResponseHandler.HandleResponseAsync(context, _dbContext, newBodyStream);

                newBodyStream.Seek(0, SeekOrigin.Begin);
                await newBodyStream.CopyToAsync(originalBodyStream);
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }
    }
}
