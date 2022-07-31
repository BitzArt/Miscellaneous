using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitzArt.EntityBase;

public abstract class EntityBase<TKey> : IIdentifiable<TKey>
{
    [Key, Column("Id", Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TKey? Id { get; set; }
}
