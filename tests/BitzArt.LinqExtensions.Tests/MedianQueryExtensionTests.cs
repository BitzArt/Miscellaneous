namespace BitzArt.LinqExtensions.Tests;

public class MedianQueryExtensionTests
{
    private class TestEntity
    {
        public int Id { get; set; }
    }

    [Fact]
    public void Median_WhenEvenNumberOfElements_ShouldReturnFirstOfMiddleTwo()
    {
        // Arrange
        var database = Enumerable.Range(1, 10).Select(x => new TestEntity { Id = x });
        var query = database.AsQueryable();

        // Act
        var result = query
            .Median(x => x.Id)
            .Single();

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Median_WhenOddNumberOfElements_ShouldReturnMiddleElement()
    {
        // Arrange
        var database = Enumerable.Range(1, 9).Select(x => new TestEntity { Id = x });
        var query = database.AsQueryable();

        // Act
        var result = query
            .Median(x => x.Id)
            .Single();

        // Assert
        Assert.Equal(5, result);
    }
}
