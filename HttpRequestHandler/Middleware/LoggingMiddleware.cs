using System.Net;
using IMiddleware = HttpRequestHandler.Middleware.Base.Interfaces.IMiddleware;

namespace HttpRequestHandler.Middleware;

/// <summary>
/// Логирование запросов
/// </summary>
public class LoggingMiddleware : IMiddleware
{
    public void Invoke(HttpListenerContext context, Action next)
    {
        Console.WriteLine($"[{DateTime.Now}] START: {context.Request.HttpMethod} {context.Request.Url}");
        
        next();

        Console.WriteLine($"[{DateTime.Now}] END: {context.Request.HttpMethod} {context.Request.Url}");
    }
}