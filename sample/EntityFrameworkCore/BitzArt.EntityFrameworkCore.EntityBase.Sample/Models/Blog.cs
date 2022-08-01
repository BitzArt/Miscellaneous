using BitzArt.EntityBase;
using System.ComponentModel.DataAnnotations;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Models
{
    public class Blog : EntityBase<Guid>
    {
        [MaxLength(256)]
        public string Name { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
