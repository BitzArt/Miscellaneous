using Microsoft.Extensions.Logging;
using Wolverine;

namespace MediaMars.Messaging;

internal static class MessageLoggingExtensions
{
    public static void LogProcessingStarted<TMessage>(this ILogger logger, IMessageContext context)
        where TMessage : class
    {
        logger.LogDebug(
             "Processing '{messageType}':\n{message}\nCorrelationId: {correlationId}",
             typeof(TMessage).Name, context.Envelope?.Message, context.CorrelationId);
    }

    public static void LogProcessingCompleted(this ILogger logger, IMessageContext context)
    {
        logger.LogDebug("Processed");
    }

    public static void LogProcessingError<TMessage>(this ILogger logger, IMessageContext context, Exception ex)
        where TMessage : class
    {
        logger.LogError(
            "Processing error of '{messageType}':\n{message}\nError: {exception}\nCorrelationId: {correlationId}",
            typeof(TMessage).Name, context.Envelope?.Message, ex, context.CorrelationId);
    }
}
