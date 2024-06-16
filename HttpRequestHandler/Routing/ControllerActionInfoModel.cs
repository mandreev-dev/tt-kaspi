namespace HttpRequestHandler.Routing;

/// <summary>
/// Информация о действии контроллера
/// </summary>
public class ControllerActionInfoModel(Type controllerType, string actionName)
{
    public Type ControllerType { get; } = controllerType;
    public string ActionName { get; } = actionName;
}