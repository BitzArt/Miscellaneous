using BitzArt.EntityBase;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Models
{
    public class User : EntityBase<Guid>
    {
        public string Name { get; set; }
    }
}
