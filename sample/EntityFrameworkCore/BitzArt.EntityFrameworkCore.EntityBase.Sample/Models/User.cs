using BitzArt.EntityBase;
using System.ComponentModel.DataAnnotations;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Models
{
    public class User : EntityCreated<Guid>
    {
        [MaxLength(64)]
        public string Name { get; set; }
    }
}
