namespace BitzArt.XUnit;

public class Tests(TransientDependency dep)
{
    [Fact]
    public void Test1()
    {
        Assert.NotNull(dep);

        Assert.True(true);
    }

    [Fact]
    public void Test2()
    {
        Assert.NotNull(dep);

        Assert.True(true);
    }
}
