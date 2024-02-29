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

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void NotEqual_Filter_Filters(int notEqualTo)
        {
            var range = Enumerable.Range(1, 10);
            var models = range.Select(x => new TestModelStruct { Id = x }).ToList();
            var queryable = models.AsQueryable();

            int? filter = notEqualTo;

            var filtered = queryable.AddFilter(x => x.Id, filter, FilterOperation.NotEqual).ToList();

            Assert.Equal(9, filtered.Count);

            if (notEqualTo != 1) Assert.True(filtered.First().Id == 1);
            else Assert.True(filtered.First().Id == 2);

            if (notEqualTo != 10) Assert.True(filtered.Last().Id == 10);
            else Assert.True(filtered.Last().Id == 9);

            Assert.DoesNotContain(filtered, x => x.Id == filter);
        }

        [Fact]
        public void GreaterThan_Filter_Filters()
        {
            var range = Enumerable.Range(1, 10);
            var models = range.Select(x => new TestModelStruct { Id = x }).ToList();
            var queryable = models.AsQueryable();

            int? filter = 5;

            var filtered = queryable.AddFilter(x => x.Id, filter, FilterOperation.GreaterThan).ToList();

            Assert.Equal(5, filtered.Count);
            Assert.True(filtered.First().Id == 6);
            Assert.True(filtered.Last().Id == 10);
        }

        [Fact]
        public void GreaterThanOrEqual_Filter_Filters()
        {
            var range = Enumerable.Range(1, 10);
            var models = range.Select(x => new TestModelStruct { Id = x }).ToList();
            var queryable = models.AsQueryable();

            int? filter = 5;

            var filtered = queryable.AddFilter(x => x.Id, filter, FilterOperation.GreaterThanOrEqual).ToList();

            Assert.Equal(6, filtered.Count);
            Assert.True(filtered.First().Id == 5);
            Assert.True(filtered.Last().Id == 10);
        }

        [Fact]
        public void LessThan_Filter_Filters()
        {
            var range = Enumerable.Range(1, 10);
            var models = range.Select(x => new TestModelStruct { Id = x }).ToList();
            var queryable = models.AsQueryable();

            int? filter = 5;

            var filtered = queryable.AddFilter(x => x.Id, filter, FilterOperation.LessThan).ToList();

            Assert.Equal(4, filtered.Count);
            Assert.True(filtered.First().Id == 1);
            Assert.True(filtered.Last().Id == 4);
        }

        [Fact]
        public void LessThanOrEqual_Filter_Filters()
        {
            var range = Enumerable.Range(1, 10);
            var models = range.Select(x => new TestModelStruct { Id = x }).ToList();
            var queryable = models.AsQueryable();

            int? filter = 5;

            var filtered = queryable.AddFilter(x => x.Id, filter, FilterOperation.LessThanOrEqual).ToList();

            Assert.Equal(5, filtered.Count);
            Assert.True(filtered.First().Id == 1);
            Assert.True(filtered.Last().Id == 5);
        }
    }
}