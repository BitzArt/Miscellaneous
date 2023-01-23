using System.Threading.Tasks;

namespace MassTransit
{
    public interface IProcessor<TMessage> where TMessage : class
    {
        Task ProcessAsync(TMessage message);
    }
}
