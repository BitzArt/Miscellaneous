namespace BitzArt.LinqExtensions.Batching.Tests;

public class BatchTests
{
    private class TestEntity
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }

    private readonly IEnumerable<TestEntity> _entities;
    private const int _defaultBatchSize = 10;

    public BatchTests()
    {
        _entities = Enumerable.Range(0, 100)
            .Select(x => new TestEntity { Id = x, Name = $"Test Entity {x}" })
            .ToList();
    }

    [Fact]
    public void Batch_WhenNoRecords_ShouldReturnEmptyBatch()
    {
        // Arrange
        var query = Enumerable.Empty<TestEntity>().AsQueryable();
        int? lastValue = null;

        // Act
        var batch1 = query.Batch(_defaultBatchSize).ToList();
        var batch2 = query.Batch(_defaultBatchSize).ByOffset(0).ToList();
        var batch3 = query.Batch(_defaultBatchSize).ByLastValue(x => x.Id, lastValue);

        // Assert
        Assert.Empty(batch1);
        Assert.Empty(batch2);
        Assert.Empty(batch3);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void Batch_WithOnlyBatchSize_ShouldReturnCorrectBatch(int size)
    {
        // Arrange
        var query = _entities.AsQueryable();

        // Act
        var batch = query.Batch(size).ToList();

        // Assert
        var expected = _entities.Take(size).ToList();
        Assert.Equal(expected, batch);
        Assert.Equal(size, batch.Count);
    }

    [Theory]
    [InlineData(5, 5, 5, 5, 9)]
    [InlineData(10, 90, 10, 90, 99)]
    [InlineData(10, 100, 0, null, null)]
    public void Batch_ByOffset_ShouldReturnCorrectBatch(
        int size,
        int offset,
        int expectedCount,
        int? expectedFirstId,
        int? expectedLastId)
    {
        // Arrange
        var query = _entities.AsQueryable();

        // Act
        var batch = query.Select(x => x.Id!.Value)
            .Batch(size).ByOffset(offset).ToList();

        // Assert
        Assert.Equal(expectedCount, batch.Count);

        if (expectedFirstId.HasValue)
            Assert.Equal(expectedFirstId, batch.FirstOrDefault());

        if (expectedLastId.HasValue)
            Assert.Equal(expectedLastId, batch.LastOrDefault());
    }

    [Theory]
    [InlineData(0, 10, OrderDirection.Ascending, 0, null, null)]
    [InlineData(10, null, OrderDirection.Ascending, 10, 0, 9)]
    [InlineData(10, null, OrderDirection.Descending, 10, 99, 90)]
    [InlineData(5, 10, OrderDirection.Ascending, 5, 11, 15)]
    [InlineData(5, 10, OrderDirection.Descending, 5, 9, 5)]
    [InlineData(10, 94, OrderDirection.Ascending, 5, 95, 99)]
    [InlineData(10, 99, OrderDirection.Ascending, 0, null, null)]
    [InlineData(10, 0, OrderDirection.Descending, 0, null, null)]
    public void Batch_ByLastValue_ShouldReturnCorrectBatch(
        int size,
        int? lastValue,
        OrderDirection orderDirection,
        int expectedCount,
        int? expectedFirstId,
        int? expectedLastId)
    {
        // Arrange
        var query = _entities.AsQueryable();

        // Act
        var batch = query.Select(x => x.Id!.Value)
            .Batch(size)
            .ByLastValue(x => x, lastValue, orderDirection)
            .ToList();

        // Assert
        Assert.Equal(expectedCount, batch.Count);

        if (expectedFirstId.HasValue)
            Assert.Equal(expectedFirstId, batch.FirstOrDefault());

        if (expectedLastId.HasValue)
            Assert.Equal(expectedLastId, batch.LastOrDefault());
    }
}
