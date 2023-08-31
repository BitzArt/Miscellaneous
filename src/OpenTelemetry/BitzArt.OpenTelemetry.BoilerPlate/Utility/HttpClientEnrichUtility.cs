using System.Diagnostics;
using System.Text.Json;

namespace BitzArt.OpenTelemetry.BoilerPlate;

internal static class HttpClientEnrichUtility
{
    internal static void EnrichWithHttpRequestMessage(Activity activity, HttpRequestMessage message)
    {
        activity.AddTag("request.headers", JsonSerializer.Serialize(message.Headers));
        activity.AddTag("request.body", message.Content?.ReadAsStringAsync().Result);
    }

    internal static void EnrichWithHttpResponseMessage(Activity activity, HttpResponseMessage message)
    {
        activity.AddTag("response.headers", JsonSerializer.Serialize(message.Headers));
        activity.AddTag("response.body", message.Content?.ReadAsStringAsync().Result);
    }
}
