using System.Threading.Tasks;

namespace BitzArt.MassTransit
{
    public interface IProcessor<TMessage> where TMessage : class
    {
        Task ProcessAsync(TMessage message);
    }
}
