namespace BitzArt.Tests;

public class RangeExtensionsTests
{
    [Theory]
    [InlineData(1, 3, 2)]
    [InlineData(0, 5, 3)]
    [InlineData(-1, 1, 0)]
    [InlineData(0, null, 1)]
    [InlineData(0, null, 1000)]
    [InlineData(0, null, int.MaxValue)]
    [InlineData(null, 0, -1)]
    [InlineData(null, 0, -1000)]
    [InlineData(null, 0, int.MinValue)]
    public void Contains_ValueWithinRange_ShouldReturnTrue(int? lowerBound, int? upperBound, int value)
    {
        // Arrange
        var range = new Range<int?>(lowerBound, upperBound);

        // Act
        var result = range.Contains(value);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(1, 3, 4)]
    [InlineData(1, 3, 0)]
    [InlineData(0, 5, 6)]
    [InlineData(0, 5, -1)]
    [InlineData(-1, 1, 2)]
    [InlineData(-1, 1, -2)]
    [InlineData(0, 10, 100)]
    [InlineData(0, null, -1)]
    [InlineData(0, null, -1000)]
    [InlineData(0, null, int.MinValue)]
    [InlineData(null, 0, 1)]
    [InlineData(null, 0, 1000)]
    [InlineData(null, 0, int.MaxValue)]
    public void Contains_ValueOutsideRange_ShouldReturnFalse(int? lowerBound, int? upperBound, int value)
    {
        // Arrange
        var range = new Range<int?>(lowerBound, upperBound);

        // Act
        var result = range.Contains(value);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(1, 3, 1)]
    [InlineData(1, 3, 3)]
    [InlineData(0, 5, 0)]
    [InlineData(0, 5, 5)]
    [InlineData(-1, 1, -1)]
    [InlineData(-1, 1, 1)]
    [InlineData(0, null, 0)]
    [InlineData(null, 0, 0)]
    public void Contains_ValueEqualsIncludedBoundary_ShouldReturnTrue(int? lowerBound, int? upperBound, int value)
    {
        // Arrange
        var range = new Range<int?>(lowerBound, upperBound);

        // Act
        var result = range.Contains(value);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(1, 3, 1)]
    [InlineData(1, 3, 3)]
    [InlineData(0, 5, 0)]
    [InlineData(0, 5, 5)]
    [InlineData(-1, 1, -1)]
    [InlineData(-1, 1, 1)]
    [InlineData(0, null, 0)]
    [InlineData(null, 0, 0)]
    public void Contains_ValueEqualsNotIncludedBoundary_ShouldReturnFalse(int? lowerBound, int? upperBound, int value)
    {
        // Arrange
        var range = new Range<int?>(lowerBound, upperBound, false, false);

        // Act
        var result = range.Contains(value);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(1, 5, 0, 10)] // All range values are within query range
    [InlineData(-3, 0, 0, 10)] // Lower boundary partially overlaps query range
    [InlineData(9, 12, 0, 10)] // Upper boundary partially overlaps query range
    [InlineData(-3, -1, 0, 10)] // Range completely outside query range (lower)
    [InlineData(10, 12, 0, 10)] // Range completely outside query range (upper)
    [InlineData(7, 12, 0, 10)] // Partial overlap with query range
    [InlineData(-2, 2, 0, 10)] // Partial overlap with query range
    [InlineData(null, 5, 0, 10)] // Open lower bound
    [InlineData(4, null, 0, 10)] // Open upper bound
    [InlineData(-1, 10, 0, 10)] // Query completely within range
    public void Where_CompareWithGetInclusionExpressionResult_ReturnsSame(
        int? lowerBound,
        int? upperBound,
        int start,
        int elementsCount)
    {
        // Arrange  
        var range = new Range<int?>(lowerBound, upperBound);
        var query = Enumerable.Range(start, elementsCount).AsQueryable();

        // Act  
        var whereResult = query.Where(x => x, range).ToList();
        var getInclExprResult = query.Where(range.GetInclusionExpression<int, int>(x => x)).ToList();

        // Assert
        Assert.Equal(whereResult, getInclExprResult);
    }
}
