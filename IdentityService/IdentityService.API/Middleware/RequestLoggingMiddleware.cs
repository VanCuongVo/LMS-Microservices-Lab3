using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.Services;

namespace IdentityService.API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {

            _next = next;

        }
        public async Task InvokeAsync(HttpContext context, ILogService logService)
        {
            var stopwatch = Stopwatch.StartNew();

            Exception? exception = null;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                stopwatch.Stop();

                var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var email = context.User?.FindFirst(ClaimTypes.Email)?.Value;

                var statusCode = context.Response.StatusCode;

                var (level, statusText) = statusCode switch
                {
                    200 => ("Information", "OK"),
                    201 => ("Information", "Created"),
                    204 => ("Information", "NoContent"),

                    301 => ("Information", "MovedPermanently"),
                    302 => ("Information", "Found"),
                    304 => ("Information", "NotModified"),

                    400 => ("Warning", "BadRequest"),
                    401 => ("Warning", "Unauthorized"),
                    403 => ("Warning", "Forbidden"),
                    404 => ("Warning", "NotFound"),
                    405 => ("Warning", "MethodNotAllowed"),
                    409 => ("Warning", "Conflict"),
                    422 => ("Warning", "UnprocessableEntity"),
                    429 => ("Warning", "TooManyRequests"),

                    500 => ("Error", "InternalServerError"),
                    501 => ("Error", "NotImplemented"),
                    502 => ("Error", "BadGateway"),
                    503 => ("Error", "ServiceUnavailable"),
                    504 => ("Error", "GatewayTimeout"),

                    _ => (statusCode >= 500 ? "Error" :
                          statusCode >= 400 ? "Warning" :
                          "Information", "Unknown")
                };

                var log = new Log
                {
                    Message = exception != null
                        ? $"EXCEPTION: {exception.Message}"
                        : $"HTTP {context.Request.Method} {context.Request.Path} => {statusCode} in {stopwatch.ElapsedMilliseconds} ms",

                    Level = exception != null ? "Error" : level,

                    UserId = userId,
                    Email = email,

                    Exception = exception?.ToString(),

                    Properties = JsonSerializer.Serialize(new
                    {
                        StatusCode = statusCode,
                        StatusText = statusText,
                        context.Request.Method,
                        context.Request.Path,
                        ExecutionTime = stopwatch.ElapsedMilliseconds
                    }),

                    TimeStamp = DateTime.UtcNow
                };

                await logService.WriteAsync(log);
            }
        }
    }
}