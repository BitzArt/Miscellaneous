namespace BitzArt.Patcher.Tests;

public class ModelExtensionTests
{
    [Fact]
    public void Model_InputInnerIsNull_ShouldSkip()
    {
        // Arrange
        var target = TestModel.GenerateTarget();
        var input = TestModel.GenerateInput();

        // Act
        target.Patch(input)
            .Model(x => x.Inner, inner => inner
                .Property(x => x.A)
                .Property(x => x.B)
                .Property(x => x.C));

        // Assert
        Assert.Null(input.Inner);
        Assert.Null(target.Inner);
    }

    [Fact]
    public void Model_InputInnerIsNotNullButEmpty_ShouldCreateNewTargetInner()
    {
        // Arrange
        var target = TestModel.GenerateTarget();
        var input = TestModel.GenerateInput();

        input.Inner = new();

        // Act
        target.Patch(input)
            .Model(x => x.Inner, inner => inner
                .Property(x => x.A)
                .Property(x => x.B)
                .Property(x => x.C));

        // Assert
        Assert.NotNull(input.Inner);
        Assert.NotNull(target.Inner);

        Assert.Null(input.Inner!.A);
        Assert.Null(input.Inner!.B);
        Assert.Null(input.Inner!.C);

        Assert.Null(target.Inner!.A);
        Assert.Null(target.Inner!.B);
        Assert.Null(target.Inner!.C);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Model_InputInnerHasAllFields_ShouldPatchAll(bool prepopulateTarget)
    {
        // Arrange
        var target = TestModel.GenerateTarget();

        if (prepopulateTarget)
        {
            target.Inner = new()
            {
                A = target.A,
                B = target.B,
                C = target.C
            };
        }

        var input = TestModel.GenerateInput();
        input.Inner = new()
        {
            A = input.A,
            B = input.B,
            C = input.C
        };

        // Act
        target.Patch(input)
            .Model(x => x.Inner, inner => inner
                .Property(x => x.A)
                .Property(x => x.B)
                .Property(x => x.C));

        // Assert
        Assert.NotNull(input.Inner);
        Assert.NotNull(target.Inner);

        Assert.Equal(input.Inner!.A, target.Inner!.A);
        Assert.Equal(input.Inner!.B, target.Inner!.B);
        Assert.Equal(input.Inner!.C, target.Inner!.C);
    }
}
