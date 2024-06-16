using Newtonsoft.Json;

namespace HttpRequestHandler.Controllers.Dto;

/// <summary>
/// Тестовая request Dto
/// </summary>
public class ExampleRequest
{
    [JsonProperty("data")]
    public required string Data { get; init; }

    [JsonProperty("count")]
    public required int Count { get; init; }
}