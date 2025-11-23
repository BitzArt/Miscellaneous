namespace BitzArt.Patcher.Tests;

internal class TestModel
{
    public string? A { get; set; }
    public string? B { get; set; }
    public string? C { get; set; }

    public NestedModel? Inner { get; set; }

    public class NestedModel : TestModel { }

    public static TestModel GenerateTarget() => new()
    {
        A = "[initial]a",
        B = "[initial]b",
        C = "[initial]c"
    };

    public static TestModel GenerateInput() => new()
    {
        A = "[patched]a",
        B = "[patched]b",
        C = "[patched]c"
    };
}