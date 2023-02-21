namespace BitzArt.LinqExtensions.Tests
{
    public class AddFilterTests
    {
        private record TestModel
        {
            public int? Id { get; set; }
        }

        [Fact]
        public void ApplyFilter_FilterNotNull_Filters()
        {
            var range = Enumerable.Range(1, 10);
            var models = range.Select(x => new TestModel { Id = x }).ToList();
            var queryable = models.AsQueryable();

            int? filter = 5;

            var filtered = queryable.AddFilter(x => x.Id, filter).ToList();

            Assert.Single(filtered);
            Assert.Contains(filtered, x => x.Id == filter);
        }

        [Fact]
        public void ApplyFilter_FilterNull_DoesNotFilter()
        {
            var range = Enumerable.Range(1, 10);
            var models = range.Select(x => new TestModel { Id = x }).ToList();
            var queryable = models.AsQueryable();

            int? filter = null;

            var filtered = queryable.AddFilter(x => x.Id, filter).ToList();

            Assert.Equal(models, filtered);
        }
    }
}