using System;
using static BitzArt.RangeMergeUtility;

namespace BitzArt;

public class RangeMergeUtilityTests
{
    [Fact]
    public void Merge_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        var ranges = new List<Range<int?>>();

        // Act
        var result = Merge(ranges);

        // Assert
        Assert.Empty(result);
    }

    [Theory]
    [InlineData(1, 2, true, true)]
    [InlineData(null, null, true, true)]
    [InlineData(null, 0, true, true)]
    [InlineData(0, null, true, true)]
    public void Merge_SingleItem_ShouldReturnSameItem(int? lowerBound, int? upperBound, bool includeLowerBound, bool includeUpperBound)
    {
        // Arrange
        var range = new Range<int?>(lowerBound, upperBound, includeLowerBound, includeUpperBound);
        var ranges = new List<Range<int?>> { range };

        // Act
        var result = Merge(ranges);

        // Assert
        Assert.Single(result);
        var resultItem = result.First();

        Assert.Equal(range.LowerBound, resultItem.LowerBound);
        Assert.Equal(range.UpperBound, resultItem.UpperBound);
        Assert.Equal(range.IncludeLowerBound, resultItem.IncludeLowerBound);
        Assert.Equal(range.IncludeUpperBound, resultItem.IncludeUpperBound);
    }

    [Fact]
    public void Merge_TwoNonOverlappingItems_ShouldReturnSameItems()
    {
        // Arrange
        var range1 = new Range<int?>(1, 2);
        var range2 = new Range<int?>(3, 4);

        var ranges = new List<Range<int?>> { range1, range2 };

        // Act
        var result = Merge(ranges);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(range1, result);
        Assert.Contains(range2, result);
    }

    [Theory]
    [InlineData(1, 3, true, true, 2, 4, true, true, 1, 4, true, true)]
    [InlineData(1, 3, true, true, 2, 4, true, false, 1, 4, true, false)]
    [InlineData(1, 3, true, true, 2, 4, false, true, 1, 4, true, true)]
    [InlineData(1, 3, false, true, 2, 4, true, true, 1, 4, false, true)]
    [InlineData(1, 3, false, false, 2, 4, true, true, 1, 4, false, true)]
    [InlineData(1, 3, false, false, 2, 4, false, false, 1, 4, false, false)]
    [InlineData(2, 4, true, true, 1, 3, true, true, 1, 4, true, true)]
    [InlineData(null, 1, true, true, -1, null, true, true, null, null, false, false)]
    [InlineData(null, 1, true, true, -1, 10, true, true, null, 10, false, true)]
    [InlineData(-10, 1, true, true, -1, null, true, true, -10, null, true, false)]
    public void Merge_TwoOverlappingItems_ShouldReturnMergedItem(
        int? lowerBound1,
        int? upperBound1,
        bool includeLowerBound1,
        bool includeUpperBound1,
        int? lowerBound2,
        int? upperBound2,
        bool includeLowerBound2,
        bool includeUpperBound2,
        int? expectedLowerBound,
        int? expectedUpperBound,
        bool expectedIncludeLowerBound,
        bool expectedIncludeUpperBound)
    {
        // Arrange
        var range1 = new Range<int?>(lowerBound1, upperBound1, includeLowerBound1, includeUpperBound1);
        var range2 = new Range<int?>(lowerBound2, upperBound2, includeLowerBound2, includeUpperBound2);

        var ranges = new List<Range<int?>> { range1, range2 };

        // Act
        var result = Merge(ranges);

        // Assert
        Assert.Single(result);
        var resultItem = result.First();

        var expected = new Range<int?>(expectedLowerBound, expectedUpperBound, expectedIncludeLowerBound, expectedIncludeUpperBound);

        Assert.Equal(expected, resultItem);

        Assert.Equal(expected.LowerBound, resultItem.LowerBound);
        Assert.Equal(expected.UpperBound, resultItem.UpperBound);
        Assert.Equal(expected.IncludeLowerBound, resultItem.IncludeLowerBound);
        Assert.Equal(expected.IncludeUpperBound, resultItem.IncludeUpperBound);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public void Merge_TwoItemsMeetingAtTheSameIncludedPoint_ShouldMergeItems(bool include1, bool include2)
    {
        // Arrange
        var range1 = new Range<int?>(1, 2, true, include1);
        var range2 = new Range<int?>(2, 3, include2, true);

        var ranges = new List<Range<int?>> { range1, range2 };

        // Act
        var result = Merge(ranges);

        // Assert
        Assert.Single(result);
        var resultItem = result.First();
        var expected = new Range<int?>(1, 3);
        Assert.Equal(expected, resultItem);
    }

    [Fact]
    public void Merge_TwoItemsMeetingAtTheSameNonIncludedPoint_ShouldNotMergeItems()
    {
        // Arrange
        var range1 = new Range<int?>(1, 2, true, false);
        var range2 = new Range<int?>(2, 3, false, true);

        var ranges = new List<Range<int?>> { range1, range2 };

        // Act
        var result = Merge(ranges);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(range1, result);
        Assert.Contains(range2, result);
    }

    [Theory]
    [InlineData(0, 5, true, true, 2, 3, true, true)]
    [InlineData(0, 5, true, true, 2, 3, true, false)]
    [InlineData(0, 5, true, true, 2, 3, false, true)]
    [InlineData(0, 5, true, true, 2, 3, false, false)]
    [InlineData(0, 5, true, false, 2, 3, true, true)]
    [InlineData(0, 5, true, false, 2, 3, true, false)]
    [InlineData(0, 5, true, false, 2, 3, false, true)]
    [InlineData(0, 5, true, false, 2, 3, false, false)]
    [InlineData(0, 5, false, true, 2, 3, true, true)]
    [InlineData(0, 5, false, true, 2, 3, true, false)]
    [InlineData(0, 5, false, true, 2, 3, false, true)]
    [InlineData(0, 5, false, true, 2, 3, false, false)]
    [InlineData(0, 5, false, false, 2, 3, true, true)]
    [InlineData(0, 5, false, false, 2, 3, true, false)]
    [InlineData(0, 5, false, false, 2, 3, false, true)]
    [InlineData(0, 5, false, false, 2, 3, false, false)]
    [InlineData(null, 5, false, false, 2, 3, true, true)]
    [InlineData(null, 5, false, false, 2, 3, true, false)]
    [InlineData(null, 5, false, false, 2, 3, false, true)]
    [InlineData(null, 5, false, false, 2, 3, false, false)]
    [InlineData(0, null, false, false, 2, 3, true, true)]
    [InlineData(0, null, false, false, 2, 3, true, false)]
    [InlineData(0, null, false, false, 2, 3, false, true)]
    [InlineData(0, null, false, false, 2, 3, false, false)]
    [InlineData(null, null, false, false, 2, 3, true, true)]
    [InlineData(null, null, false, false, 2, 3, true, false)]
    [InlineData(null, null, false, false, 2, 3, false, true)]
    [InlineData(null, null, false, false, 2, 3, false, false)]
    public void Merge_OneItemFullyInsideAnother_ShouldReturnOuterOne(
        int? outerLower,
        int? outerUpper,
        bool outerIncludeLower,
        bool outerIncludeUpper,
        int innerLower,
        int innerUpper,
        bool innerIncludeLower,
        bool innerIncludeUpper)
    {
        // Arrange
        var outer = new Range<int?>(outerLower, outerUpper, outerIncludeLower, outerIncludeUpper);
        var inner = new Range<int?>(innerLower, innerUpper, innerIncludeLower, innerIncludeUpper);

        var ranges = new List<Range<int?>> { outer, inner };

        // Act
        var result = Merge(ranges);

        // Assert
        Assert.Single(result);
        var resultItem = result.First();

        Assert.Equal(outer.LowerBound, resultItem.LowerBound);
        Assert.Equal(outer.UpperBound, resultItem.UpperBound);
        Assert.Equal(outer.IncludeLowerBound, resultItem.IncludeLowerBound);
        Assert.Equal(outer.IncludeUpperBound, resultItem.IncludeUpperBound);
    }
}
