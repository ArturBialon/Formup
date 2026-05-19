using Application.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Net;
using System.Text.Json;

namespace Application.Middleware
{
    public class ExceptionMiddleware(
        RequestDelegate next,
        ILogger logger,
        IHostEnvironment env)
    {
        public readonly RequestDelegate _next = next;
        public readonly ILogger _logger = logger;
        public readonly IHostEnvironment _env = env;
        public readonly JsonSerializerOptions _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (ex is FluentValidation.ValidationException validationException)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    var errors = validationException.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(x => x.ErrorMessage).ToArray()
                        );

                    var validationResponse = new
                    {
                        Title = "Fluent validation exception occured.",
                        Status = 400,
                        Errors = errors
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(validationResponse));
                    return;
                }

                _logger.Error(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString() ?? "NULL_TRACE")
                    : new ApiException(context.Response.StatusCode);

                var json = JsonSerializer.Serialize(response, _jsonOptions);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
