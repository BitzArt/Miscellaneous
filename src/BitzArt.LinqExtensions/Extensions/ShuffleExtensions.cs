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
    /// <param name="source">The collection to shuffle.</param>
    /// <param name="seed">The seed to use for the random number generator.</param>
    /// <returns>A new IQueryable with the elements shuffled.</returns>
    public static IQueryable<TSource> Shuffle<TSource>(this IQueryable<TSource> source, int? seed = null)
        => source.Shuffle(seed).AsQueryable();

    /// <summary>
    /// Shuffles the elements of the collection
    /// using <see href="https://en.wikipedia.org/wiki/Fisher-Yates_shuffle"/>
    /// </summary>
    /// <param name="source">The collection to shuffle.</param>
    /// <param name="seed">The seed to use for the random number generator.</param>
    /// <returns>A new collection with the elements shuffled.</returns>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, int? seed = null)
    {
        var random = seed.HasValue ? new Random(seed.Value) : _random;
        return source.Shuffle(random);
    }

    /// <summary>
    /// Shuffles the elements of the collection
    /// using <see href="https://en.wikipedia.org/wiki/Fisher-Yates_shuffle"/>
    /// </summary>
    /// <param name="source">The collection to shuffle.</param>
    /// <param name="random">The random number generator to use.</param>
    /// <returns>A new collection with the elements shuffled.</returns>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random random)
    {
        // preserve the original collection
        var list = new List<T>(source);

        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
        return list;
    }
}
