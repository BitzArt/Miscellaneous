namespace BitzArt.LinqExtensions.Tests;

public class WhereConditionBuilderTests
{
    [Fact]
    public static void TestMethod()
    {
        var originalData = Enumerable.Range(1, 10).Select(x => new TestClass { Property = x }).ToList().AsQueryable();

        var range = new Range<int>(2, 3);

        var result = originalData!.Where(x => x.Property).IsTrue(x => x.In(range)).ToList();
    }
}
