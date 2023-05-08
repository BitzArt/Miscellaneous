using BitzArt.Patcher;

namespace BitzArt;

public static class PatchExtension
{
    public static IPatchBuilder<TModel>? Patch<TModel>(this TModel target, TModel input)
        where TModel : class
    {
        if (input is null) return null;

        return new PatchBuilder<TModel>(target, input);
    }
}
