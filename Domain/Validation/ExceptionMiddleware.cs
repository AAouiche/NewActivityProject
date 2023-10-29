using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Hosting;

namespace Domain.Validation
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            if (exception is FluentValidation.ValidationException validationException)
            {
                // Validation exception handling
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var validationErrors = string.Join(", ", validationException.Errors.Select(err => $"{err.PropertyName}: {err.ErrorMessage}"));
                var response = new AppException(context.Response.StatusCode, "Validation failed", validationErrors);

                var jsonResponse = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(jsonResponse);
            }
            else
            {
                // General exception handling
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new AppException(context.Response.StatusCode, exception.Message, exception.StackTrace?.ToString())
                    : new AppException(context.Response.StatusCode, "Internal Server Error");

                var jsonResponse = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}