using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitzArt.EntityBase;

[Owned]
public class CreationInfo
{
    [Column("CreatedOn")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedOn { get; set; }

    private protected CreationInfo() { }

    internal CreationInfo(DateTime? createdOn = null)
    {
        if (createdOn is null) createdOn = DateTime.UtcNow;
        CreatedOn = createdOn.Value;
    }
}

[Owned]
public class CreationInfo<TCreatorKey> : CreationInfo
    where TCreatorKey : struct
{
    [Column("CreatedBy")]
    public TCreatorKey? CreatorId { get; set; }

    private protected CreationInfo() { }

    internal CreationInfo(TCreatorKey creatorId, DateTime? createdOn = null) : base(createdOn)
    {
        ArgumentNullException.ThrowIfNull(creatorId);
        CreatorId = creatorId;
    }
}

[Owned]
public class CreationInfo<TCreator, TCreatorKey> : CreationInfo<TCreatorKey>
    where TCreatorKey : struct
    where TCreator : IIdentifiable<TCreatorKey>
{
    [ForeignKey(nameof(CreatorId))]
    public TCreator? Creator { get; set; }

    private protected CreationInfo() { }

    internal CreationInfo(TCreatorKey creatorId, DateTime? createdOn = null) : base(creatorId, createdOn) { }

    internal CreationInfo(TCreator creator, DateTime? createdOn = null) : this(creator.Id!.Value, createdOn)
    {
        Creator = creator;
    }
}
