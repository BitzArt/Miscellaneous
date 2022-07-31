using BitzArt.EntityBase;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Models
{
    public class Product : EntityUpdated<Guid, User, Guid>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ICollection<Category>? Categories { get; set; }

        public Product() { }

        public Product(User creator) : base(creator) { }
    }
}