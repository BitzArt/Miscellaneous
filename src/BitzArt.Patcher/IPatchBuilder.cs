namespace BitzArt.Patcher
{
    public interface IPatchBuilder<T>
    {
        T Target { get; }
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
}
