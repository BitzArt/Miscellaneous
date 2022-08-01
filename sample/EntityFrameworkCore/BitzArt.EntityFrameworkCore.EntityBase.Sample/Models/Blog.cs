using BitzArt.EntityBase;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Models
{
    public class Blog : EntityBase<Guid>
    {
        public string Name { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
