using Microsoft.Extensions.Logging;

namespace MassTransit
{
    public class ConsumerBase<TProcessor, TMessage> : ConsumerBase<TMessage>
        where TProcessor : IProcessor<TMessage>
        where TMessage : class
    {
        public ConsumerBase(ILogger<IConsumer<TMessage>> logger, TProcessor processor) : base(logger, processor)
        {
        }
    }
}
