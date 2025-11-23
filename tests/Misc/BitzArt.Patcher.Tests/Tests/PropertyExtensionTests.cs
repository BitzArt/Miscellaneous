namespace BitzArt.Patcher.Tests;

public class PropertyExtensionTests
{
    [Fact]
    public void Property_InputHasAllFields_ShouldPatchAll()
    {
        var target = TestModel.GenerateTarget();
        var input = TestModel.GenerateInput();

        // Act
        target.Patch(input)
            .Property(x => x.A)
            .Property(x => x.B)
            .Property(x => x.C);

        // Assert
        Assert.Equal(input.A, target.A);
        Assert.Equal(input.B, target.B);
        Assert.Equal(input.C, target.C);
    }

    [Fact]
    public void Property_InputHasNullFields_ShouldSkipNull()
    {
        // Arrange
        var target = TestModel.GenerateTarget();
        var targetC = new string(target.C);

        var input = TestModel.GenerateInput();
        input.C = null;

        // Act
        target.Patch(input)
            .Property(x => x.A)
            .Property(x => x.B)
            .Property(x => x.C);

        // Assert
        Assert.Equal(input.A, target.A);
        Assert.Equal(input.B, target.B);

        Assert.NotNull(target.C);
        Assert.Equal(targetC, target.C);
    }
}
