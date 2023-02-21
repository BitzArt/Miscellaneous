using BitzArt;
using System.Linq.Expressions;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static IQueryable<TSource> AddFilter<TSource, TProperty>(this IQueryable<TSource> source, Expression<Func<TSource, TProperty?>> expression, TProperty? filter)
            where TProperty : struct
        {
            if (filter is null) return source;

            var filterValue = filter!.Value;

            Expression<Func<TProperty?, TProperty>> getValueExpression = x => x!.Value;

            var argument = Expression.Parameter(typeof(TSource));
            var valueExpression = expression.Compose(getValueExpression);
            var left = Expression.Invoke(valueExpression, argument);
            var right = Expression.Constant(filterValue);
            var eq = Expression.Equal(left, right);

            var lambda = Expression
                .Lambda<Func<TSource, bool>>(eq, new[] { argument });

            return source.Where(lambda);
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (condition) return source.Where(predicate);
            return source;
        }
    }
}
