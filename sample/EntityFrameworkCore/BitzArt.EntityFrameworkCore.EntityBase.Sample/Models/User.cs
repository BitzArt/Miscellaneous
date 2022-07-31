using BitzArt.EntityBase;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Models
{
    public class User : EntityCreated<Guid>
    {
        public string Name { get; set; }
    }
}
