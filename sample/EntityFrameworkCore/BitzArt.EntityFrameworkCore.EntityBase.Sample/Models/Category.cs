using BitzArt.EntityBase;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Models
{
    public class Category : EntityBase<Guid>
    {
        public string Name { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
