namespace BitzArt.Patcher.Tests;

public class PatchTests
{
    private class TestModel
    {
        public string? A { get; set; }
        public string? B { get; set; }
        public string? C { get; set; }
    }

    private static TestModel GenerateTarget() => new()
    {
        A = "a",
        B = "b",
        C = "c"
    };

    private static TestModel GenerateInput() => new()
    {
        A = "1",
        B = "2",
        C = "3"
    };

    [Fact]
    public void Patch_AllFields_Patches()
    {
        var target = GenerateTarget();
        var input = GenerateInput();

        target.Patch(input)
            .Property(x => x.A)
            .Property(x => x.B)
            .Property(x => x.C);

        Assert.Equal(input.A, target.A);
        Assert.Equal(input.B, target.B);
        Assert.Equal(input.C, target.C);
    }

    [Fact]
    public void Patch_WithNullFields_SkipsNull()
    {
        var target = GenerateTarget();
        var targetC = new string(target.C);

        var input = GenerateInput();
        input.C = null;

        target.Patch(input)
            .Property(x => x.A)
            .Property(x => x.B)
            .Property(x => x.C);

        Assert.Equal(input.A, target.A);
        Assert.Equal(input.B, target.B);

        Assert.NotNull(target.C);
        Assert.Equal(targetC, target.C);
    }
}