using System.Text.Json.Serialization;

namespace BitzArt.Probes;

internal class ServiceInfoResponse
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("environment")]
    public string? Environment { get; set; }

    [JsonPropertyName("startedAt")]
    public DateTimeOffset StartedAt { get; set; }

    [JsonPropertyName("currentTime")]
    public static DateTimeOffset CurrentTime => DateTimeOffset.UtcNow;

    [JsonPropertyName("uptime")]
    public TimeSpan Uptime => CurrentTime - StartedAt;

    public ServiceInfoResponse(ServiceInfoOptions? options, string? environment)
    {
        Name = options?.Name;
        Version = options?.Version;
        Environment = environment;

        StartedAt = DateTimeOffset.UtcNow;
    }
}
