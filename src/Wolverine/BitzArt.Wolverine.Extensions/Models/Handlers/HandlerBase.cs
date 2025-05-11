using Microsoft.Extensions.Logging;
using Wolverine;

namespace BitzArt.Messages;

/// <summary>
/// A base class for message consumers.<br/>
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="HandlerBase{TMessage}"/> class.
/// </remarks>
public abstract class HandlerBase<TMessage>(ILogger logger) : IWolverineHandler
    where TMessage : class
{
    private readonly ILogger _logger = logger;

    // ========================== MESSAGE CONSUMPTION ==========================

    /// <summary>
    /// Handles the received message by executing <see cref="ProcessAsync(TMessage, IMessageContext)"/> on it.
    /// </summary>
    /// <param name="message">Message to be processed.</param>
    /// <param name="context">Message context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(TMessage message, IMessageContext context)
    {
        try
        {
            _logger.LogProcessingStarted<TMessage>(context);

            await ProcessAsync(message, context);

            _logger.LogProcessingCompleted(context);
        }
        catch (Exception ex)
        {
            _logger.LogProcessingError<TMessage>(context, ex);
            throw;
        }
    }

    // ========================== MESSAGE PROCESSING ==========================

    /// <summary>
    /// This is an abstract method and must be implemented in derived classes
    /// to provide specific message processing logic.
    /// </summary>
    protected abstract Task ProcessAsync(TMessage message, IMessageContext context);
}
