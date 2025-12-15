using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using community_service_api.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace community_service_api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;
        var requestPath = context.Request.Path;
        var requestMethod = context.Request.Method;

        switch (exception)
        {
            case BusinessRuleException businessRuleException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new { error = businessRuleException.Message });
                logger.LogWarning("Excepción de regla de negocio en {Method} {Path}: {Message}",
                    requestMethod, requestPath, businessRuleException.Message);
                break;
            case DataAccessException dataAccessException:
                code = HttpStatusCode.InternalServerError;
                result = JsonSerializer.Serialize(new { error = dataAccessException.Message });
                logger.LogError(dataAccessException, "Excepción de acceso a datos en {Method} {Path}",
                    requestMethod, requestPath);
                break;
            case not null:
                logger.LogError(exception, "Excepción no controlada en {Method} {Path}",
                    requestMethod, requestPath);
                // En producción, no exponer detalles del error
                var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
                if (isDevelopment)
                {
                    var errorDetails = new { error = exception.Message, type = exception.GetType().ToString(), stackTrace = exception.StackTrace };
                    result = JsonSerializer.Serialize(errorDetails);
                }
                else
                {
                    result = JsonSerializer.Serialize(new { error = "Ha ocurrido un error interno. Por favor, intente más tarde." });
                }
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsync(result);
    }
}
