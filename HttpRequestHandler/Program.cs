using System.Net;
using System.Reflection;
using HttpRequestHandler.Helpers;
using HttpRequestHandler.Middleware;
using HttpRequestHandler.Middleware.Base;
using HttpRequestHandler.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Controller = HttpRequestHandler.Controllers.Base.Controller;

namespace HttpRequestHandler;

public class Program
{
    private static readonly ControllerActionService _routerService = new();
    private static readonly MiddlewarePipeline _middlewarePipeline = new();

    static void Main(string[] args)
    {
        AddControllers();
        AddMiddlewares();
        Initialize();
    }

    /// <summary>
    /// Добавление контроллеров
    /// </summary>
    private static void AddControllers()
    {
        var controllerTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Controller)))
            .ToList();

        foreach (var type in controllerTypes)
        {
            var controllerMethodList = type
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.DeclaringType == type);

            var path = string.Empty;
            var classRouteAttribute = type.GetCustomAttribute<RouteAttribute>();
            if (classRouteAttribute != null)
            {
                path = classRouteAttribute.Template;
            }

            foreach (var method in controllerMethodList)
            {
                var methodRouteAttribute = method.GetCustomAttributes<HttpMethodAttribute>().FirstOrDefault();
                if (methodRouteAttribute == null)
                {
                    throw new Exception("the request must contain the HttpMethodAttribute attribute");
                }

                var requestPath = path;
                if (methodRouteAttribute.Template != null)
                {
                    requestPath = path + "/" + methodRouteAttribute.Template;
                }

                var httpMethod = methodRouteAttribute.HttpMethods.FirstOrDefault() ?? "GET";
                _routerService.RegisterByRoute(requestPath, httpMethod, type, method.Name);
            }
        }
    }

    /// <summary>
    /// Добавление middleware
    /// </summary>
    private static void AddMiddlewares()
    {
        // Пример добавления Middleware
        _middlewarePipeline.Use(new LoggingMiddleware());
    }

    /// <summary>
    /// Инициализация сервиса
    /// </summary>
    private static void Initialize()
    {
        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/");
        listener.Start();
        Console.WriteLine("Application started.");

        while (true)
        {
            var context = listener.GetContext();

            _middlewarePipeline.Execute(context);

            if (TryHandleRequest(context, out var responseString))
            {
                HttpRequestHelper.WriteResponseBody(context, responseString);
            }
        }
    }

    /// <summary>
    /// Логика обработки запроса
    /// </summary>
    private static bool TryHandleRequest(HttpListenerContext context, out string response)
    {
        try
        {
            var request = context.Request;

            var pathWithoutSlash = request.Url.AbsolutePath[1..];
            var httpMethod = request.HttpMethod;

            var route = _routerService.GetInfo(pathWithoutSlash, httpMethod);

            var controller = Activator.CreateInstance(route.ControllerType);
            var method = route.ControllerType.GetMethod(route.ActionName);

            var headers = request.Headers.AllKeys.ToDictionary(key => key, key => request.Headers[key]);

            var parameters = method.GetParameters();
            var parameterValues = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].GetCustomAttribute<FromHeaderAttribute>() != null)
                {
                    parameterValues[i] = headers.GetValueOrDefault(parameters[i].Name) ?? string.Empty;
                }
                else if (parameters[i].GetCustomAttribute<FromBodyAttribute>() != null)
                {
                    if (httpMethod != HttpMethod.Post.Method)
                    {
                        throw new InvalidOperationException("The requested resource does not support HTTP method GET with request body.");
                    }

                    using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                    var requestBody = reader.ReadToEnd();
                    var data = JsonConvert.DeserializeObject(requestBody, parameters[i].ParameterType);
                    parameterValues[i] = data;
                }
            }

            response = (string)method.Invoke(controller, parameterValues);
            return true;
        }
        catch (Exception e)
        {
            HttpRequestHelper.WriteResponseException(context, e);
            response = string.Empty;
            return false;
        }
    }
}