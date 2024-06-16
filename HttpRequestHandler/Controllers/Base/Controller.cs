using Newtonsoft.Json;

namespace HttpRequestHandler.Controllers.Base;

public abstract class Controller
{
    protected string JsonResponse(object response)
    {
        return JsonConvert.SerializeObject(response);
    }
}