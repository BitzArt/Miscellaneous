using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitzArt.EntityBase;

[Owned]
public class UpdateInfo
{
    [Column("UpdatedOn")]
    public DateTime UpdatedOn { get; set; }

    internal UpdateInfo(DateTime? updatedOn = null)
    {
        if (updatedOn is null) updatedOn = DateTime.UtcNow;
        UpdatedOn = updatedOn.Value;
    }

    internal UpdateInfo() : this(null) { }
}

[Owned]
public class UpdateInfo<TUpdaterKey> : UpdateInfo
    where TUpdaterKey : struct
{
    [Column("UpdatedBy")]
    public TUpdaterKey? UpdaterId { get; set; }

    internal UpdateInfo() : base() { }

    internal UpdateInfo(TUpdaterKey updaterId, DateTime? updatedOn = null) : base(updatedOn)
    {
        ArgumentNullException.ThrowIfNull(updaterId);
        UpdaterId = updaterId;
    }
}

[Owned]
public class UpdateInfo<TUpdater, TUpdaterKey> : UpdateInfo<TUpdaterKey>
    where TUpdaterKey : struct
    where TUpdater : IIdentifiable<TUpdaterKey>
{
    [ForeignKey(nameof(UpdaterId))]
    public TUpdater? Updater { get; set; }

    internal UpdateInfo() : base() { }

    internal UpdateInfo(TUpdaterKey updaterId, DateTime? updatedOn = null) : base(updaterId, updatedOn) { }

    internal UpdateInfo(TUpdater updater, DateTime? updatedOn = null) : this(updater.Id!.Value, updatedOn)
    {
        Updater = updater;
    }
}
