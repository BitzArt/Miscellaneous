using BitzArt.ApiExceptions;
using Wolverine;
using Microsoft.Extensions.Logging;

namespace MediaMars.Messaging;

/// <summary>
/// A base class for request message consumers.<br/>
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RequestProcessor{TRequestMessage}"/> class.
/// </remarks>
public abstract class RequestProcessor<TRequestMessage>(ILogger logger) : IRequestProcessor<TRequestMessage>
    where TRequestMessage : class
{
    private readonly ILogger _logger = logger;

    // ========================== MESSAGE CONSUMPTION ==========================

    /// <summary>
    /// Responds to the requester by returning a result of executing <see cref="ProcessAsync(TRequestMessage, IMessageContext)"/> on the received message.
    /// </summary>
    /// <param name="message">Message to be processed.</param>
    /// <param name="context">Message context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<ResponseMessage> HandleAsync(TRequestMessage message, IMessageContext context)
    {
        try
        {
            _logger.LogProcessingStarted<TRequestMessage>(context);

            var result = await ProcessAsync(message, context);

            _logger.LogProcessingCompleted(context);

            return result;
        }
        catch (ApiExceptionBase ex)
        {
            _logger.LogProcessingError<TRequestMessage>(context, ex);

            // ApiExceptions are converted to response messages
            // with an appropriate status code
            return ex.ToResponseMessage();
        }
        catch (Exception ex)
        {
            _logger.LogProcessingError<TRequestMessage>(context, ex);

            // Re-throwing an exception here will result
            // in an exception being raised in the caller
            throw;
        }
    }

    // ========================== MESSAGE PROCESSING ==========================

    /// <summary>
    /// This is an abstract method and must be implemented in derived classes
    /// to provide specific message processing logic.
    /// </summary>
    protected abstract Task<ResponseMessage> ProcessAsync(TRequestMessage message, IMessageContext context);

    // ========================== GENERATING RESPONSES ==========================

    /// <summary>
    /// Generates a response message with 'OK' status code.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    protected ResponseMessage<TPayload> Ok<TPayload>(TPayload data)
        where TPayload : class
    {
        return new ResponseMessage<TPayload>(data);
    }

    /// <summary>
    /// Generates a response message with the specified status code.
    /// </summary>
    protected ResponseMessage<TPayload> StatusCode<TPayload>(ApiStatusCode apiStatusCode, TPayload data)
        where TPayload : class
    {
        return new ResponseMessage<TPayload>(apiStatusCode, data);
    }

    /// <summary>
    /// Generates a response message with the specified status code.
    /// </summary>
    protected ResponseMessage<TPayload> StatusCode<TPayload>(int statusCode, TPayload data)
        where TPayload : class
    {
        return new ResponseMessage<TPayload>(statusCode, data);
    }
}
