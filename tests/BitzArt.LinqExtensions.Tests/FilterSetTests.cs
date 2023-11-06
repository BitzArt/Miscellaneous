namespace BitzArt.LinqExtensions.Tests;

public class FilterSetTests
{
    private class TestEntity
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }

    private class TestFilterSet : IFilterSet<TestEntity>
    {
        public int? Id { get; set; }
        public string? Name { get; set; }

        public IQueryable<TestEntity> Apply(IQueryable<TestEntity> query)
        {
            return query
                .AddFilter(x => x.Id, Id)
                .AddFilter(x => x.Name, Name);
        }
    }

    [Fact]
    public void Apply_WithIdFilter_Filters()
    {
        var filterSet = new TestFilterSet
        {
            Id = 1
        };

        var query = Enumerable.Range(1, 10)
            .Select(x => new TestEntity { Id = x })
            .AsQueryable();

        var result = query.Apply(filterSet);

        Assert.Equal(1, result.Count());
    }

    [Fact]
    public void Apply_WithNameFilter_Filters()
    {
        var filterSet = new TestFilterSet
        {
            Name = "Test 1"
        };

        var query = Enumerable.Range(1, 10)
            .Select(x => new TestEntity { Name = $"Test {x}" })
            .AsQueryable();

        var result = query.Apply(filterSet);

        Assert.Equal(1, result.Count());

        var entity = result.Single();

        Assert.Equal("Test 1", entity.Name);
    }
}
