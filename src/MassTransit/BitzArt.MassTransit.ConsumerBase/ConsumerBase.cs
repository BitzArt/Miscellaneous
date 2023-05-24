using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MassTransit
{
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

        public virtual async Task Consume(ConsumeContext<TMessage> context) =>
            await Processor.ProcessAsync(context.Message);

        public virtual Task Consume(ConsumeContext<Fault<TMessage>> context)
        {
            var errors = context.Message.Exceptions.Select(x => x.Message);
            var json = JsonSerializer.Serialize(errors);
            Logger.LogError("Message consume attempt failed. Errors:\n{json}", json);
            return Task.CompletedTask;
        }
    }
}
