using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using community_service_api.Exceptions;
using Microsoft.AspNetCore.Http;

namespace community_service_api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError; // 500 if unexpected
        var result = string.Empty;

        switch (exception)
        {
            case BusinessRuleException businessRuleException:
                code = HttpStatusCode.BadRequest; // 400 for business rule violations
                result = JsonSerializer.Serialize(new { error = businessRuleException.Message });
                break;
            case DataAccessException dataAccessException:
                code = HttpStatusCode.InternalServerError; // Or a more specific code if applicable
                result = JsonSerializer.Serialize(new { error = dataAccessException.Message });
                break;
            case not null:
                // Temporarily expose full error for debugging.
                var errorDetails = new { error = exception.Message, type = exception.GetType().ToString(), stackTrace = exception.StackTrace };
                result = JsonSerializer.Serialize(errorDetails);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}
