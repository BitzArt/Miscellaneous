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
    public void Batch_WhenNoRecords_ShouldReturnEmptyBatch()
    {
        // Arrange
        var query = Enumerable.Empty<TestEntity>().AsQueryable();
        int? lastValue = null;

        // Act
        var batch = query.Batch(x => x.Id, _batchSize, lastValue).ToList();

        // Assert
        Assert.Empty(batch);
    }

    [Fact]
    public void Batch_WithOnlyBatchSize_ShouldReturnOnlyFirstBatch()
    {
        // Arrange
        var query = _entities.AsQueryable();

        // Act
        var batch = query.Batch(x => x.Id, _batchSize).ToList();

        // Assert
        var expected = _entities.Take(_batchSize).ToList();
        Assert.Equal(expected, batch);
    }

    [Theory]
    [InlineData(null, 10, 0, 9)]
    [InlineData(0, 10, 1, 10)]
    [InlineData(9, 10, 10, 19)]
    [InlineData(98, 1, 99, 99)]
    [InlineData(99, 0, null, null)]
    public void Batch_WithLastValue_ShouldReturnCorrectBatch(
        int? lastValue,
        int expectedCount,
        int? expectedFirstId,
        int? expectedLastId)
    {
        // Arrange
        var query = _entities.AsQueryable();

        // Act
        var batch = query.Select(x => x.Id!.Value)
            .Batch(x => x, _batchSize, lastValue).ToList();

        // Assert
        Assert.Equal(expectedCount, batch.Count);

        if (expectedFirstId.HasValue)
            Assert.Equal(expectedFirstId, batch.FirstOrDefault());

        if (expectedLastId.HasValue)
            Assert.Equal(expectedLastId, batch.LastOrDefault());
    }

    [Theory]
    [InlineData(5, 5, 5, 5, 9)]
    [InlineData(10, 90, 10, 90, 99)]
    [InlineData(10, 100, 0, null, null)]
    public void Batch_WithSizeAndOffset_ShouldReturnCorrectBatch(
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
            .Batch(x => x, size, offset).ToList();

        // Assert
        Assert.Equal(expectedCount, batch.Count);

        if (expectedFirstId.HasValue)
            Assert.Equal(expectedFirstId, batch.FirstOrDefault());

        if (expectedLastId.HasValue)
            Assert.Equal(expectedLastId, batch.LastOrDefault());
    }

    [Theory]
    [InlineData(OrderDirection.Ascending)]
    [InlineData(OrderDirection.Descending)]
    public void Batch_WithOrderDirection_ShouldReturnCorrectBatch(OrderDirection orderDirection)
    {
        // Arrange
        var query = _entities.AsQueryable();

        // Act
        var batch = query.Batch(x => x.Id, _batchSize, orderDirection).ToList();

        // Assert
        var expected = orderDirection == OrderDirection.Ascending
            ? _entities.Take(_batchSize).ToList()
            : _entities.TakeLast(_batchSize).Reverse().ToList();

        Assert.Equal(expected, batch);
    }
}
