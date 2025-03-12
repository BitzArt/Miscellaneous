namespace BitzArt.CoreExtensions.Tests;

public class InExtensionTests
{
    [Theory]
    [MemberData(nameof(GetTestData))]
    public void In_WithGivenValues_ShouldReturnCorrectResult<T>(T value, T? lowerBound, T? upperBound, bool includeLowerBound, bool includeUpperBound, bool expectedResult)
       where T : struct, IComparable<T>
    {
        // Arrange
        var range = new Range<T>
        {
            LowerBound = lowerBound,
            UpperBound = upperBound,
            IncludeLowerBound = includeLowerBound,
            IncludeUpperBound = includeUpperBound
        };

        // Act
        var result = value.In(range);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    public static IEnumerable<object?[]> GetTestData()
    {
        return
        [
            // In the range
            [5, 1, 10, true, true, true],
            [7, 7, 7, true, true, true],
            ['e', 'a', 'f', true, false, true],
            [2, -10, 10, false, true, true],
            [9.9, 1.0, 10.0, false, false, true],
            [6.0, 1.0, null, true, true, true],
            [-7, null, null, true, true, true],
            [0, null, 10, true, true, true],
            [5, null, 10, true, true, true],
            [15, 1, null, true, true, true],
            [new DateTime(2025, 5, 3), new DateTime(2025, 5, 1), new DateTime(2025, 5, 5), false, false, true],
            [new DateTime(2025, 5, 3), new DateTime(2025, 5, 3), new DateTime(2025, 5, 5), true, false, true],
            [new DateTime(2025, 5, 3), null, new DateTime(2025, 5, 5), false, false, true],

            // Out of the range
            [9, 9, 9, false, false, false],
            [15.3, 1.0, 5.1, true, true, false],
            [10, 1, 10, true, false, false],
            [1, 1, 10, false, true, false],
            ['g', 'a', 'f', false, false, false],
            ['a', 'a', 'f', false, false, false],
            [11, null, 10, true, true, false],
            [-7, -2, 8, true, true, false],
            [-5, 1, null, true, true, false],
            [new DateTime(2025, 5, 3), new DateTime(2025, 5, 3), new DateTime(2025, 5, 5), false, false, false],
            [new DateTime(2025, 5, 1), new DateTime(2025, 5, 3), new DateTime(2025, 5, 5), true, true, false],
            [new DateTime(2025, 5, 1), new DateTime(2025, 5, 3), null, true, false, false],
        ];
    }
}
