using System.Net;

namespace HttpRequestHandler.Middleware.Base.Interfaces;

public interface IMiddleware
{
    void Invoke(HttpListenerContext context, Action next);
}