using System.Linq;

namespace BitzArt;

public interface IFilterSet<TSource> where TSource : class
{
    public IQueryable<TSource> Apply(IQueryable<TSource> query);
}
