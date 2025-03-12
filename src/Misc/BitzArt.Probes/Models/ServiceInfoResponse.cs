using System.Text.Json.Serialization;

namespace BitzArt.Probes;

internal class ServiceInfoResponse(ServiceInfoOptions? options, string? environment)
{
    [JsonPropertyName("name")]
    public string? Name { get; set; } = options?.Name;

    [JsonPropertyName("version")]
    public string? Version { get; set; } = options?.Version;

    [JsonPropertyName("environment")]
    public string? Environment { get; set; } = environment;
}
