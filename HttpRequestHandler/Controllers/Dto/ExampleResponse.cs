using Newtonsoft.Json;

namespace HttpRequestHandler.Controllers.Dto;

/// <summary>
/// Тестовая response Dto
/// </summary>
public class ExampleResponse
{
    [JsonProperty("name")]
    public required string Name { get; init; }

    [JsonProperty("count")]
    public required int Count { get; init; }
}