using Elastic.Extensions.Logging;
using MassTransit.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace BitzArt;

public static class ElasticApmExtensions
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

    public static WebApplicationBuilder AddTelemetry(this WebApplicationBuilder builder)
    {
        var section = builder.Configuration.GetSection("ElasticApm");
        if (!section.Exists()) return builder;

        var apmUrl = section.GetValue<string>("ServerUrl")!;
        var environment = section.GetValue<string>("Environment")!;
        var serviceName = section.GetValue<string>("ServiceName")!;

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName)
            .AddTelemetrySdk()
            .AddAttributes(
                new KeyValuePair<string, object>[]
                    {
                        new("deployment.environment", environment)
                    })
            .AddEnvironmentVariableDetector();

        builder.Logging
            .AddOpenTelemetry(x =>
            {
                x.SetResourceBuilder(resourceBuilder)
                    .AddOtlpExporter(cfg =>
                    {
                        cfg.Endpoint = new Uri(apmUrl);
                    });

                x.IncludeFormattedMessage = true;
                x.IncludeScopes = true;
                x.ParseStateValues = true;
            });

        builder.Services.AddOpenTelemetry()
        .WithTracing(trace =>
        {
            trace
                .AddSource(DiagnosticHeaders.DefaultListenerName)
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation(o =>
                {
                    o.RecordException = true;
                })
                .AddSqlClientInstrumentation(o =>
                {
                    o.EnableConnectionLevelAttributes = true;
                    o.RecordException = true;
                    o.SetDbStatementForText = true;
                    o.SetDbStatementForStoredProcedure = true;
                })
                .AddHttpClientInstrumentation(o =>
                {
                    o.RecordException = true;
                })
                .AddOtlpExporter(cfg =>
                {
                    cfg.Endpoint = new Uri(apmUrl);
                });
        });

        ExceptionTelemetry.EnableOpenTelemetry();

        return builder;
    }
}
