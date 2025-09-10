namespace BitzArt;

/// <summary>
/// Transient service provider options.
/// </summary>
public class TransientServiceProviderOptions
{
    /// <summary>
    /// Use global service provider if service missing in transient service provider.
    /// </summary>
    public bool FallbackToGlobal { get; set; } = false;
}