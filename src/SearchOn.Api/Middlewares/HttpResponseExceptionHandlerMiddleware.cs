using SearchOn.Shared.Exceptions;
using System.Net;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace SearchOn.Api.Middlewares;

public class HttpResponseExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public HttpResponseExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IHostEnvironment hostEnvironment, ILogger logger)
    {
        context.Response.Headers.Add("Request-ID", context.TraceIdentifier);

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var exception = UnWrapException(ex);

            if (ex is not LogicException)
            {
                logger.Error(exception, "");
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (hostEnvironment.IsDevelopment())
            {
                await context.Response.WriteAsJsonAsync(exception);
            }
            else
            {
                await context.Response.WriteAsJsonAsync("An unexpected error occurred.");
            }
        }

        Exception UnWrapException(Exception exp)
        {
            if (exp is TargetInvocationException)
                return exp.InnerException ?? exp;

            return exp;
        }
    }
}

public static class HttpResponseExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseHttpResponseExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HttpResponseExceptionHandlerMiddleware>();
    }
}
