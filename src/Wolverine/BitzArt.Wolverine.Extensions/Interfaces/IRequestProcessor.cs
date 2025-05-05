using Wolverine;

namespace MediaMars.Messaging;

/// <summary>
/// Defines the contract for a message processor that processes messages
/// of the specified type and returns <see cref="ResponseMessage"/>.
/// </summary>
/// <typeparam name="TMessage">
/// </typeparam>
public interface IRequestProcessor<in TMessage> : IWolverineHandler
{
    /// <summary>
    /// Wolverine compatible method to process the message.
    /// <seealso href="https://wolverinefx.net/guide/handlers/discovery.html#handler-type-discovery"/>
    /// </summary>
    /// <param name="message">
    /// Message to be processed.
    /// </param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Task<ResponseMessage> HandleAsync(TMessage message, IMessageContext context);
}