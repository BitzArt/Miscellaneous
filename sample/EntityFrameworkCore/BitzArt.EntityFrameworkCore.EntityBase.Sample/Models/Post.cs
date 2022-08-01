using BitzArt.EntityBase;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Models
{
    public class Post : EntityUpdated<Guid, User, Guid>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ICollection<Blog> Blogs { get; set; }

        public Post() { }

        public Post(User creator) : base(creator) { }
    }
}