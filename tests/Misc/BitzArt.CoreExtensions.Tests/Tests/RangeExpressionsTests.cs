using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BitzArt.Tests;

public class RangeExpressionsTests
{
    private class TestEntity
    {
        public int? Id { get; set; }

        public ICollection<TestOwnedEntity>? Owned { get; set; }
    }

    private class TestOwnedEntity
    {
        public int? Id { get; set; }
    }

    private class TestDbContext : DbContext
    {
        private static SqliteConnection _connection = null!;

        private static Lock _connectionLock = new();

        private static void CheckConnection()
        {
            if (_connection is not null) return;

            lock (_connectionLock)
            {
                if (_connection is not null) return;

                _connection = new SqliteConnection("Filename=:memory:");
                _connection.Open();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            CheckConnection();

            optionsBuilder.UseSqlite(_connection!);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestEntity>()
                .HasMany(x => x.Owned)
                .WithOne();

            modelBuilder.Entity<TestOwnedEntity>();
        }
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(0, 5)]
    [InlineData(-1, 1)]
    [InlineData(0, null)]
    [InlineData(null, 0)]
    [InlineData(null, null)]
    public void ToQueryString_GetRangeInclusionExpression_ShouldBuildQueryString(int? lowerBound, int? upperBound)
    {
        // Arrange
        var dbContext = new TestDbContext();
        var range = new Range<int?>(lowerBound, upperBound);

        // Act
        var queryString = dbContext.Set<TestEntity>()
            .Where(range.GetInclusionExpression<TestEntity, int>(x => x.Id!.Value))
            .ToQueryString();

        // Assert
        Assert.NotEmpty(queryString);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(0, 5)]
    [InlineData(-1, 1)]
    [InlineData(0, null)]
    [InlineData(null, 0)]
    [InlineData(null, null)]
    public void ToQueryString_GetRangeEnumerableInclusionExpression_ShouldBuildQueryString(int? lowerBound, int? upperBound)
    {
        // Arrange
        var dbContext = new TestDbContext();
        var ranges = new Range<int?>[] { new(lowerBound, upperBound), new(lowerBound, upperBound) };

        // Act
        var queryString = dbContext.Set<TestEntity>()
            .Where(ranges.GetInclusionExpression<TestEntity, int>(x => x.Id!.Value))
            .ToQueryString();

        // Assert
        Assert.NotEmpty(queryString);
    }
}
