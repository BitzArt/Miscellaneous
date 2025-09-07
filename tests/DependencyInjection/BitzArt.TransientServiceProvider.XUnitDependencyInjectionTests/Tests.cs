namespace BitzArt.XUnit;

// TransientDependency is added to the transient service provider only.
public class Tests(TransientDependency thisDependency)
{
    [Fact]
    public void DependencyInjection_ViaCtor_ShouldProvideTransientDependency()
    {
        // Assert
        Assert.NotNull(thisDependency);
    }
}
