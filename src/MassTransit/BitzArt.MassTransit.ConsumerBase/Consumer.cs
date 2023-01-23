using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MassTransit;
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
