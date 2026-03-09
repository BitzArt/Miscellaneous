using System.Text.RegularExpressions;

namespace BitzArt.Extensions;


/// <summary>
/// Extension methods strings.
/// </summary>
public static partial class StringExtensions
{
    [GeneratedRegex(@"\s+")]
    private static partial Regex MultiSpaceRegex();
    
    /// <summary>
    /// Removes leading and trailing white-space characters from the current string and,
    /// when requested, normalizes consecutive white-space characters to a single space.
    /// </summary>
    /// <param name="input">The string to process.</param>
    /// <param name="normalize">
    /// <see langword="true"/> to collapse multiple white-space characters into a single space;
    /// otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>
    /// A trimmed string whose internal white-space is optionally normalized,
    /// or <see langword="null"/> if <paramref name="input"/> is <see langword="null"/>.
    /// </returns>
    public static string? Trim(this string? input, bool normalize)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        return normalize ? MultiSpaceRegex().Replace(input.Trim(), " ") : input.Trim();
    }
}