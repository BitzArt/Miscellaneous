using System.Text.Json.Serialization;

namespace BitzArt.Probes;

internal class ServiceInfoResponse(ServiceInfoOptions options)
{
    [JsonPropertyName("serviceName")]
    public string? ServiceName { get; set; } = options.Name;

    [JsonPropertyName("version")]
    public string? Version { get; set; } = options.Version;
}
