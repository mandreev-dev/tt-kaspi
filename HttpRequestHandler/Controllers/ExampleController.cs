using HttpRequestHandler.Controllers.Dto;
using Microsoft.AspNetCore.Mvc;
using Controller = HttpRequestHandler.Controllers.Base.Controller;

namespace HttpRequestHandler.Controllers;

/// <summary>
/// Тестовый контролер
/// </summary>
[Route("example")]
public class ExampleController : Controller
{
    /// <summary>
    /// Пример пустого GET запроса
    /// </summary>
    /// <returns></returns>
    [HttpGet("empty")]
    public string ExampleEmptyGet()
    {
        return JsonResponse(new
        {
            message = "Response from GET method"
        });
    }
    
    /// <summary>
    /// Пример GET запроса с типизируемым телом ответа
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public string ExampleGet()
    {
        return JsonResponse(new
        {
            response = new ExampleResponse
            {
                Name = "Mike",
                Count = 1
            }
        });
    }

    /// <summary>
    /// Пример запроса с чтением параметра из заголовка
    /// </summary>
    /// <param name="myHeader"></param>
    /// <returns></returns>
    [HttpGet("with-header")]
    public string ExamplePostWithHeader(
        [FromHeader] string myHeader)
    {
        return JsonResponse(new
        {
            message = $"Response from Post method with header = {myHeader}"
        });
    }

    /// <summary>
    /// Пример запроса с типизируемым параметром из тела запроа
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public string ExamplePostWithBody(
        [FromBody] ExampleRequest request)
    {
        return JsonResponse(new
        {
            requestData = request
        });
    }
}