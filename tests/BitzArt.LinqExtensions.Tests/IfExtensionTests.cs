namespace BitzArt.LinqExtensions.Tests;

public class IfExtensionTests
{
    private class TestEntity
    {
        public int Id { get; set; }
    }

    [Fact]
    public static void If_WhenConditionTrue_ShouldApplyExpression()
    {
        // Arrange
        var database = Enumerable.Range(1, 10).Select(x => new TestEntity { Id = x });
        var query = database.AsQueryable();

        // Act
        var result = query
            .If(true, x => x.Where(e => e.Id == 1))
            .ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
    }

    [Fact]
    public static void If_WhenConditionFalse_ShouldNotApplyExpression()
    {
        // Arrange
        var database = Enumerable.Range(1, 10).Select(x => new TestEntity { Id = x });
        var query = database.AsQueryable();

        // Act
        var result = query
            .If(false, x => x.Where(e => e.Id == 1))
            .ToList();

        // Assert
        Assert.Equal(10, result.Count);
    }
}
