namespace BitzArt.LinqExtensions.Tests
{
    public class WhereIfTests
    {
        [Fact]
        public void WhereIf_WithConditionTrue_Filters()
        {
            var apply = true;

            var initialList = Enumerable.Range(1, 10).ToList();
            var queryable = initialList.AsQueryable();
            var whereList = queryable.Where(x => x > 5).ToList();
            var whereIfList = queryable.WhereIf(apply, x => x > 5).ToList();

            Assert.NotEqual(initialList, whereList);
            Assert.Equal(whereList, whereIfList);
        }

        [Fact]
        public void WhereIf_WithConditionFalse_Skips()
        {
            var apply = false;

            var initialList = Enumerable.Range(1, 10).ToList();
            var queryable = initialList.AsQueryable();
            var whereList = queryable.Where(x => x > 5).ToList();
            var whereIfList = queryable.WhereIf(apply, x => x > 5).ToList();

            Assert.NotEqual(whereList, whereIfList);
            Assert.Equal(initialList, whereIfList);
        }
    }
}