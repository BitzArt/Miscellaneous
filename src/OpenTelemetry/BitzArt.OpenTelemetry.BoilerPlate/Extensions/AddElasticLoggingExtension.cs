using Elastic.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace BitzArt;

public static class AddElasticLoggingExtension
{
    public static WebApplicationBuilder AddElasticLogging(this WebApplicationBuilder builder)
    {
        var section = builder.Configuration.GetSection("ElasticApm");
        if (!section.Exists()) return builder;

        var nodeUri = section.GetValue<string>("ElasticsearchNodeUri")!;
        var nodeUris = new List<Uri> { new Uri(nodeUri) }.ToArray();

        var environment = section.GetValue<string>("Environment")!;
        var serviceName = section.GetValue<string>("ServiceName")!;

        builder.Logging
            .AddElasticsearch(x =>
            {
                x.Tags = new List<string> { environment, serviceName }.ToArray();
                x.ShipTo.NodeUris = nodeUris;
            });


        return builder;
    }
}
