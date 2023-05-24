using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.MassTransit.ConsumerBase.Tests
{
    internal class TestMessage { }

    internal class TestMessageProcessor : IProcessor<TestMessage>
    {
        public Task ProcessAsync(TestMessage message)
        {
            throw new NotImplementedException();
        }
    }

    internal interface ITestProcessorsAssemblyPointer { }

    public class AddProcessorsTests
    {
        [Fact]
        public void AddProcessors_OnThisAssembly_AddsProcessors()
        {
            var services = new ServiceCollection();
            services.AddProcessors<ITestProcessorsAssemblyPointer>();
            var serviceProvider = services.BuildServiceProvider();

            var resolvedDirect = serviceProvider.GetRequiredService<TestMessageProcessor>();
            Assert.NotNull(resolvedDirect);

            var resolvedByInterface = serviceProvider.GetRequiredService<IProcessor<TestMessage>>();
            Assert.NotNull(resolvedByInterface);
        }
    }
}