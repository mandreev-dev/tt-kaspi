namespace HttpRequestHandler.Routing;

/// <summary>
/// Сервис ответственный за маршрутизацию запросов
/// </summary>
public class ControllerActionService
{
    /// <summary>
    /// Список маршрутов
    /// </summary>
    private readonly Dictionary<(string,string),ControllerActionInfoModel> routeList = new();

    /// <summary>
    /// Добавление действие контроллера по пути
    /// </summary>
    public void RegisterByRoute(string path, string httpMethod, Type controllerType, string actionName)
    {
        if (routeList.ContainsKey((path, httpMethod)))
        {
            throw new Exception("The route already exists");
        }
        
        routeList.Add((path, httpMethod), new ControllerActionInfoModel(controllerType, actionName));
    }

    /// <summary>
    /// Получение информации о действии контроллера
    /// </summary>
    public ControllerActionInfoModel GetInfo(string path, string httpMethod)
    {
        if (routeList.TryGetValue((path, httpMethod), out var result))
        {
            return result;
        }

        throw new Exception($"Route by path = {path} and httpMethod = {httpMethod} not found");
    }
}
