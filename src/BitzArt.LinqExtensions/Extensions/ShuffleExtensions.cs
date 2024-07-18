using System;
using System.Collections.Generic;
using System.Linq;

namespace BitzArt;

/// <summary>
/// Extension methods for shuffling collections.
/// </summary>
public static class ShuffleExtensions
{
    private static readonly Random _random = new();

    /// <summary>
    /// Shuffles the elements of the collection
    /// using <see href="https://en.wikipedia.org/wiki/Fisher-Yates_shuffle"/>
    /// </summary>
    /// <returns>A new IQueryable with the elements shuffled.</returns>
    public static IQueryable<TSource> Shuffle<TSource>(this IQueryable<TSource> source)
        => source.Shuffle().AsQueryable();

    /// <summary>
    /// Shuffles the elements of the collection
    /// using <see href="https://en.wikipedia.org/wiki/Fisher-Yates_shuffle"/>
    /// </summary>
    /// <returns>A new collection with the elements shuffled.</returns>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        // preserve the original collection
        var list = new List<T>(source);

        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = _random.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
        return list;
    }
}
