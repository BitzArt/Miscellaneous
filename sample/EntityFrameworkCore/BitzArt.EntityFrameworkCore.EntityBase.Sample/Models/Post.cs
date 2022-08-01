using BitzArt.EntityBase;
using System.ComponentModel.DataAnnotations;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Models
{
    public class Post : EntityUpdated<Guid, User, Guid>
    {
        [MaxLength(256)]
        public string Name { get; set; }

        public string Content { get; set; }

        public ICollection<Blog> Blogs { get; set; }

        public Post() { }

        public Post(User creator) : base(creator) { }
    }
}