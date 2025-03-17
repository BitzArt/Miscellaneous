namespace BitzArt.Tests;

public class RangeInclusionExpressionsTests
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
    public void InclusionExpression_ValueWithinRange_ShouldReturnTrue(int? lowerBound, int? upperBound, int value)
    {
        // Arrange
        var range = new Range<int?>(lowerBound, upperBound);

        // Act
        var result = range.GetInclusionExpression().Compile().Invoke(value);

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
    public void InclusionExpression_ValueOutsideRange_ShouldReturnFalse(int? lowerBound, int? upperBound, int value)
    {
        // Arrange
        var range = new Range<int?>(lowerBound, upperBound);

        // Act
        var result = range.GetInclusionExpression().Compile().Invoke(value);

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
    public void InclusionExpression_ValueEqualsIncludedBoundary_ShouldReturnTrue(int? lowerBound, int? upperBound, int value)
    {
        // Arrange
        var range = new Range<int?>(lowerBound, upperBound);

        // Act
        var result = range.GetInclusionExpression().Compile().Invoke(value);

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
    public void InclusionExpression_ValueEqualsNotIncludedBoundary_ShouldReturnFalse(int? lowerBound, int? upperBound, int value)
    {
        // Arrange
        var range = new Range<int?>(lowerBound, upperBound, false, false);

        // Act
        var result = range.GetInclusionExpression().Compile().Invoke(value);

        // Assert
        Assert.False(result);
    }
}
