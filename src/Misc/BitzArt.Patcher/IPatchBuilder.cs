namespace BitzArt.Patcher;

/// <summary>
/// Patch builder.
/// </summary>
/// <typeparam name="T">Type of the object being patched.</typeparam>
public interface IPatchBuilder<T>
{
    /// <summary>
    /// Target object to be patched.
    /// </summary>
    T Target { get; }

    /// <summary>
    /// Input object containing patch values.
    /// </summary>
    T Input { get; }
}

internal class PatchBuilder<T> : IPatchBuilder<T>
{
    public T Target { get; private set; }
    public T Input { get; private set; }

    public PatchBuilder(T target, T input)
    {
        Target = target;
        Input = input;
    }
}
