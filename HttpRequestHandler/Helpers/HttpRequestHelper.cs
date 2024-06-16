using System.Net;
using System.Text;

namespace HttpRequestHandler.Helpers;

/// <summary>
/// Для работы с http запросами
/// </summary>
public class HttpRequestHelper
{
    
    /// <summary>
    /// Запись тела ответа запроса
    /// </summary>
    public static void WriteResponseException(HttpListenerContext context, Exception exception)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
           
        Console.WriteLine(exception.Message);
            
        WriteResponseBody(context, exception.Message);
    }
    /// <summary>
    /// Запись тела ответа запроса
    /// </summary>
    public static void WriteResponseBody(HttpListenerContext context, string responseBody)
    {
        var response = context.Response;
        
        var buffer = Encoding.UTF8.GetBytes(responseBody);

        response.ContentLength64 = buffer.Length;
        var output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
    }
}