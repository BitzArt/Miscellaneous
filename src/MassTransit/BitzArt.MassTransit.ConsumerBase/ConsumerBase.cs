using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BitzArt.MassTransit
{
    public class ConsumerBase<TMessage> : IConsumer<TMessage>, IConsumer<Fault<TMessage>>
        where TMessage : class
    {
        private readonly ILogger _logger;
        private readonly IProcessor<TMessage> _processor;

        public ConsumerBase(ILogger<IConsumer<Fault<TMessage>>> logger, IProcessor<TMessage> processor)
        {
            _logger = logger;
            _processor = processor;
        }

        public virtual async Task Consume(ConsumeContext<TMessage> context) =>
            await _processor.ProcessAsync(context.Message);

        public virtual Task Consume(ConsumeContext<Fault<TMessage>> context)
        {
            _logger.LogError(context.Message.Exceptions[0].Message);
            return Task.CompletedTask;
        }
    }
}
