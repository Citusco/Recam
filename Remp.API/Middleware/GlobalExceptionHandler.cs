using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Remp.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        int statusCode = exception switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status403Forbidden,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError,
        };

        ProblemDetails problemDetails = new()
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Detail = exception.Message,
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    public static string GetTitle(int statusCode) =>
        statusCode switch
        {
            400 => "Bad Request",
            403 => "Forbidden",
            404 => "Not Found",
            _ => "Internal Service Error",
        };
}
