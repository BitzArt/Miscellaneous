using BitzArt.Patcher;
using System.Linq.Expressions;
using System.Reflection;

namespace BitzArt;

public static class PropertyExtension
{
    public static IPatchBuilder<TModel>? Property<TModel, TProperty>(this IPatchBuilder<TModel>? builder, Expression<Func<TModel, TProperty>> selector)
        where TModel : class
    {
        if (builder is null) return builder;

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
