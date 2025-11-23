using BitzArt.Patcher;
using System.Linq.Expressions;
using System.Reflection;

namespace BitzArt;

/// <summary>
/// Extension methods for patching objects.
/// </summary>
public static class PatcherExtensions
{
    /// <summary>
    /// Creates a patch builder for the target object using the input object.
    /// </summary>
    /// <typeparam name="T">Type of the object to be patched.</typeparam>
    /// <param name="target">Target object to be patched.</param>
    /// <param name="input">Input object containing patch values.</param>
    /// <returns>A new patch builder.</returns>
    public static IPatchBuilder<T> Patch<T>(this T target, T input)
        where T : class
    {
        return new PatchBuilder<T>(target, input);
    }

    /// <summary>
    /// Patches a nested entity within the target object using the corresponding entity from the input object.
    /// </summary>
    /// <typeparam name="TOuter">Outer object type.</typeparam>
    /// <typeparam name="TInner">Inner object type.</typeparam>
    /// <param name="builder">Patch builder.</param>
    /// <param name="selector">Inner property selector expression.</param>
    /// <param name="patch">Inner property patch action.</param>
    /// <returns>The original patch builder to allow for method chaining.</returns>
    public static IPatchBuilder<TOuter> Model<TOuter, TInner>(this IPatchBuilder<TOuter> builder, Expression<Func<TOuter, TInner?>> selector, Action<IPatchBuilder<TInner>> patch)
        where TOuter : class
        where TInner : class, new()
    {
        var prop = GetPropertyInfo(selector);

        if (builder.Input is null) return builder;

        var input = (TInner?)prop.GetValue(builder.Input);

        if (input is null) return builder;

        var target = (TInner?)prop.GetValue(builder.Target);

        if (target is null)
        {
            target = new();
            prop.SetValue(builder.Target, target);
        }

        var innerBuilder = new PatchBuilder<TInner>(target, input);

        patch.Invoke(innerBuilder);

        return builder;
    }

    /// <summary>
    /// Updates a specific property of the target object if the corresponding property in the input object is not null.
    /// </summary>
    /// <typeparam name="TModel">Type of the model being patched.</typeparam>
    /// <typeparam name="TProperty">Type of the property being patched.</typeparam>
    /// <param name="builder">Patch builder.</param>
    /// <param name="selector">Property selector expression.</param>
    /// <returns>The original patch builder to allow for method chaining.</returns>
    public static IPatchBuilder<TModel> Property<TModel, TProperty>(this IPatchBuilder<TModel> builder, Expression<Func<TModel, TProperty>> selector)
        where TModel : class
    {
        if (builder.Input is null) return builder;

        var prop = GetPropertyInfo(selector);

        var patchValue = (TProperty?)prop.GetValue(builder.Input);
        if (patchValue is null) return builder;

        prop.SetValue(builder.Target, patchValue);

        return builder;
    }

    private static PropertyInfo GetPropertyInfo<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
    {
        var expr = (MemberExpression)expression.Body;
        return (PropertyInfo)expr.Member;
    }
}
