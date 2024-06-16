using System.Net;
using IMiddleware = HttpRequestHandler.Middleware.Base.Interfaces.IMiddleware;

namespace HttpRequestHandler.Middleware.Base;

/// <summary>
/// Логика работы с middleware
/// </summary>
public class MiddlewarePipeline
{
    private readonly List<IMiddleware> _middlewares = new();

    /// <summary>
    /// Добавление нового
    /// </summary>
    public void Use(IMiddleware middleware)
    {
        _middlewares.Add(middleware);
    }

    /// <summary>
    /// Выполнение
    /// </summary>
    public void Execute(HttpListenerContext context)
    {
        var current = 0;

        void Next()
        {
            if (current < _middlewares.Count)
            {
                var middleware = _middlewares[current];
                current++;
                middleware.Invoke(context, Next);
            }
        }

        Next();
    }
}