using System.Text.Json;

namespace BitzArt.Json.TypedValues.Tests;

public class ComparisonOperatorTests
{
    [Fact]
    public void EqualityOperator_BothTypedNull_ShouldReturnTrue()
    {
        TypedValue? value1 = null;
        TypedValue? value2 = null;

        bool result = value1 == value2;

        Assert.True(result);
    }

    [Fact]
    public void EqualityOperator_LeftTypedBothNull_ShouldReturnTrue()
    {
        TypedValue? value1 = null;
        object? value2 = null;

        bool result = value1 == value2;

        Assert.True(result);
    }

    [Fact]
    public void EqualityOperator_RightTypedBothNull_ShouldReturnTrue()
    {
        object? value1 = null;
        TypedValue? value2 = null;

        bool result = value1 == value2;

        Assert.True(result);
    }

    [Fact]
    public void EqualityOperator_LeftNull_ShouldReturnFalse()
    {
        TypedValue? value1 = null;
        TypedValue? value2 = "test";

        bool result = value1 == value2;

        Assert.False(result);
    }

    [Fact]
    public void EqualityOperator_RightNull_ShouldReturnFalse()
    {
        TypedValue? value1 = "test";
        TypedValue? value2 = null;

        bool result = value1 == value2;

        Assert.False(result);
    }

    [Fact]
    public void EqualityOperator_BothTypedSameValue_ShouldReturnTrue()
    {
        TypedValue value1 = "test";
        TypedValue value2 = "test";

        bool result = value1 == value2;

        Assert.True(result);
    }

    [Fact]
    public void EqualityOperator_BothTypedDifferentValue_ShouldReturnFalse()
    {
        TypedValue value1 = "test";
        TypedValue value2 = "other";

        bool result = value1 == value2;

        Assert.False(result);
    }

    [Fact]
    public void EqualityOperator_LeftTypedSameValue_ShouldReturnTrue()
    {
        TypedValue value1 = "test";
        string value2 = "test";

        bool result = value1 == value2;

        Assert.True(result);
    }

    [Fact]
    public void EqualityOperator_LeftTypedDifferentValue_ShouldReturnFalse()
    {
        TypedValue value1 = "test";
        string value2 = "other";

        bool result = value1 == value2;

        Assert.False(result);
    }

    [Fact]
    public void EqualityOperator_RightTypedSameValue_ShouldReturnTrue()
    {
        string value1 = "test";
        TypedValue value2 = "test";

        bool result = value1 == value2;

        Assert.True(result);
    }

    [Fact]
    public void EqualityOperator_RightTypedDifferentValue_ShouldReturnFalse()
    {
        string value1 = "test";
        TypedValue value2 = "other";

        bool result = value1 == value2;

        Assert.False(result);
    }
}
