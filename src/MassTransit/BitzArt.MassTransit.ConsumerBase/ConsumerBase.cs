﻿using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MassTransit;

public abstract class ConsumerBase<TMessage> : IConsumer<TMessage>, IConsumer<Fault<TMessage>>
    where TMessage : class
{
    protected readonly ILogger Logger;
    protected readonly IProcessor<TMessage> Processor;

    public ConsumerBase(ILogger<IConsumer<TMessage>> logger, IProcessor<TMessage> processor)
    {
        Logger = logger;
        Processor = processor;
    }

    public virtual async Task Consume(ConsumeContext<TMessage> context)
    {
        Logger.LogInformation("Received message: {type}", typeof(TMessage).Name);

        await OnBeforeConsuming(context);

        var sw = Stopwatch.StartNew();
        try
        {
            await Processor.ProcessAsync(context.Message);
        }
        catch (Exception ex)
        {
            Logger.LogError("Message processing failed: {error}", ex.Message);
            throw;
        }
        Logger.LogInformation("Message has been processed successfully in {ms}ms.", sw.ElapsedMilliseconds);
        sw.Stop();

        await OnAfterConsuming(context);
    }

    public virtual Task OnBeforeConsuming(ConsumeContext<TMessage> context)
    {
        return Task.CompletedTask;
    }
    public virtual Task OnAfterConsuming(ConsumeContext<TMessage> context)
    {
        return Task.CompletedTask;
    }

    public virtual Task Consume(ConsumeContext<Fault<TMessage>> context)
    {
        var errors = context.Message.Exceptions.Select(x => x.Message);
        var json = JsonSerializer.Serialize(errors,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });
        Logger.LogError("Message processing failed after {count} attempts. Errors:\n{json}", errors.Count(), json);
        return Task.CompletedTask;
    }
}
