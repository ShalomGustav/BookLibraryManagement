using Newtonsoft.Json;
using System.Net;

namespace BookLibraryManagement.Middlewares;

public class ErrorHandlingMiddleware
{
    //Пример работы с Middleware, для данного проекта излишне, просто демонстрация работы. 
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorDetails = new
        {
            Message = "Произошла ошибка при обработке запроса",
            Detailed = ex.Message,
            ExceptionType = ex.GetType().Name,
        };

        var result = JsonConvert.SerializeObject(errorDetails);
        return context.Response.WriteAsync(result);
    }
}
