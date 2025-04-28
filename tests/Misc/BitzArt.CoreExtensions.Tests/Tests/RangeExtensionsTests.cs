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
    // All range values are included => All values are included
    [InlineData(1, 3, 0, 9, 3)]
    // One boundary value is included => One value is included
    [InlineData(-1, 0, 0, 9, 1)]
    // One boundary value is included => One value is included
    [InlineData(9, 10, 0, 9, 1)]
    // All range values are excluded => No values are included
    [InlineData(-3, -1, 0, 9, 0)]
    // All range values are excluded => No values are included
    [InlineData(10, 12, 0, 9, 0)]
    // Range values are partially included => Only values in range are included. Boundary value is included as well
    [InlineData(7, 12, 0, 9, 3)]
    // Range values are partially included => Only values in range are included. Boundary value is included as well
    [InlineData(-2, 2, 0, 9, 3)]
    public void WhereInRange_IncludedBoundary_ShouldFilterOut(
        int lowerBound,
        int upperBound,
        int start,
        int end,
        int expectedCount)
    {
        var elementsCount = end - start + 1;

        var query = Enumerable.Range(start, elementsCount).AsQueryable();
        var range = new Range<int?>(lowerBound, upperBound);

        var result = query.Where(x => x, range).ToList();

        Assert.Equal(expectedCount, result.Count);
    }

    [Theory]
    // All range values are included => All values except boundary values are included
    [InlineData(1, 5, 0, 9, 3)]
    // One boundary value is included => No values are included
    [InlineData(-1, 0, 0, 9, 0)]
    // One boundary value is included => No values are included
    [InlineData(9, 10, 0, 9, 0)]
    // All range values are excluded => No values are included
    [InlineData(-3, -1, 0, 9, 0)]
    // All range values are excluded => No values are included
    [InlineData(10, 12, 0, 9, 0)]
    // Range values are partially included => Only values in range are included ([8, 9]). Boundary value is not included (7)
    [InlineData(7, 12, 0, 9, 2)]
    // Range values are partially included => Only values in range are included ([0, 1]). Boundary value is not included (2)
    [InlineData(-2, 2, 0, 9, 2)]
    public void WhereInRange_NotIncludedBoundary_ShouldFilterOut(
        int lowerBound,
        int upperBound,
        int start,
        int end,
        int expectedCount)
    {
        var elementsCount = end - start + 1;

        var query = Enumerable.Range(start, elementsCount).AsQueryable();
        var range = new Range<int?>(lowerBound, upperBound, false, false);

        var result = query.Where(x => x, range).ToList();

        Assert.Equal(expectedCount, result.Count);
    }
}
