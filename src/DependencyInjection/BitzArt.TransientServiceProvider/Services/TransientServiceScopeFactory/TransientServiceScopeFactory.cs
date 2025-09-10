using Microsoft.Extensions.DependencyInjection;

namespace BitzArt;

/// <summary>
/// An implementation of <see cref="IServiceScopeFactory"/>
/// that makes any service scope created from it
/// use a new <see cref="ITransientServiceProvider"/> instance as its root provider.
/// </summary>
/// <param name="globalServiceProvider"></param>
public class TransientServiceScopeFactory(IServiceProvider globalServiceProvider) : IServiceScopeFactory
{
    /// <inheritdoc/>
    public IServiceScope CreateScope()
    {
        return new TransientScope(globalServiceProvider);
    }

    private class TransientScope : IServiceScope
    {
        private readonly IServiceScope _internalScope;

        public IServiceProvider ServiceProvider { get; private init; }

        public TransientScope(IServiceProvider globalServiceProvider)
        {
            _internalScope = globalServiceProvider.CreateScope();
            ServiceProvider = _internalScope.ServiceProvider.GetRequiredService<ITransientServiceProvider>();
        }

        public void Dispose() => _internalScope.Dispose();
    }
}
