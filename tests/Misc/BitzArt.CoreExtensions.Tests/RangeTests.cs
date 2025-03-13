using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using BitzArt.Extensions;

namespace BitzArt.CoreExtensions;

public class RangeTests
{
    private class TestEntity
    {
        public int Id { get; set; }

        public ICollection<TestOwnedEntity> Owned { get; set; }
    }

    private class TestOwnedEntity
    {
        public int Id { get; set; }
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

    [Fact]
    public void ToQueryString_GetRangeInclusionExpression_ShouldBuildQueryString()
    {
        // Arrange
        var dbContext = new TestDbContext();
        var range = new Range<int>(1, 10);

        // Act
        var queryString = dbContext.Set<TestEntity>()
            .Where(range.GetInclusionExpression<TestEntity, int>(x => x.Id))
            .ToQueryString();

        // Assert
        Assert.NotEmpty(queryString);
    }

    [Fact]
    public void ToQueryString_GetRangeEnumerableInclusionExpression_ShouldBuildQueryString()
    {
        // Arrange
        var dbContext = new TestDbContext();
        var ranges = new Range<int>[] { new(1, 2), new(2, 3), new(3, 4) };

        // Act
        var queryString = dbContext.Set<TestEntity>()
            .Where(ranges.GetInclusionExpression<TestEntity, int>(x => x.Id))
            .ToQueryString();

        // Assert
        Assert.NotEmpty(queryString);
    }
}
