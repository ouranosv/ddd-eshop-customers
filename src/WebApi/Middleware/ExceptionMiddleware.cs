using System;
using System.Threading.Tasks;
using Common.Dtos;
using Common.Helpers;
using Infrastructure.Services.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonHelper.Serialize(new ErrorMessage(exception.Message)));

                logger.LogError("\r\nMessage: {0}\r\nStackTrace: {1}", exception.Message, exception.StackTrace);
            }
        }
    }
}
