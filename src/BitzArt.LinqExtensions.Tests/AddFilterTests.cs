namespace BitzArt.LinqExtensions.Tests
{
    public class AddFilterTests
    {
        private record TestModelClass
        {
            public string? Id { get; set; }
        }

        private record TestModelStruct
        {
            public int? Id { get; set; }
        }

        [Fact]
        public void ClassFilter_FilterNotNull_Filters()
        {
            var range = new List<string> { "a", "b", "c", "d", "e", "f", "g" };
            var models = range.Select(x => new TestModelClass { Id = x }).ToList();
            var queryable = models.AsQueryable();

            string? filter = "c";

            var filtered = queryable.AddFilter(x => x.Id, filter).ToList();

            Assert.Single(filtered);
            Assert.Contains(filtered, x => x.Id == filter);
        }

        [Fact]
        public void ClassFilter_FilterNull_DoesNotFilter()
        {
            var range = new List<string> { "a", "b", "c", "d", "e", "f", "g" };
            var models = range.Select(x => new TestModelClass { Id = x }).ToList();
            var queryable = models.AsQueryable();

            string? filter = null;

            var filtered = queryable.AddFilter(x => x.Id, filter).ToList();

            Assert.Equal(models, filtered);
        }

        [Fact]
        public void StructFilter_FilterNotNull_Filters()
        {
            var range = Enumerable.Range(1, 10);
            var models = range.Select(x => new TestModelStruct { Id = x }).ToList();
            var queryable = models.AsQueryable();

            int? filter = 5;

            var filtered = queryable.AddFilter(x => x.Id, filter).ToList();

            Assert.Single(filtered);
            Assert.Contains(filtered, x => x.Id == filter);
        }

        [Fact]
        public void StructFilter_FilterNull_DoesNotFilter()
        {
            var range = Enumerable.Range(1, 10);
            var models = range.Select(x => new TestModelStruct { Id = x }).ToList();
            var queryable = models.AsQueryable();

            int? filter = null;

            var filtered = queryable.AddFilter(x => x.Id, filter).ToList();

            Assert.Equal(models, filtered);
        }
    }
}