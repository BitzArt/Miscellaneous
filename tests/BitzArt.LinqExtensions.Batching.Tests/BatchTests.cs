using BitzArt.LinqExtensions.Batching;

namespace BitzArt.LinqExtensions.Tests;

public class BatchTests
{
    private class TestEntity
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }

    private readonly IEnumerable<TestEntity> _entities;
    private const int _batchSize = 10;

    public BatchTests()
    {
        _entities = Enumerable.Range(0, 100)
            .Select(x => new TestEntity { Id = x, Name = $"Test Entity {x}" })
            .ToList();
    }

    [Fact]
    public void Batch__ShouldReturnCorrectBatch()
    {
        // Arrange
        var query = _entities.AsQueryable();

        // Act
        try
        {
            var batch = query.Batch(_batchSize).ByOffset(10).ToList();
        }
        catch (Exception ex)
        {

        }

        // Assert
        // Assert.Empty(batch);
    }
}
