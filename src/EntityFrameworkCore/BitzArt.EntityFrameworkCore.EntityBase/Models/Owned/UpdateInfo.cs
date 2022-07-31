using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitzArt.EntityBase
{
    [Owned]
    public class UpdateInfo
    {
        [Column("UpdatedOn")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedOn { get; set; }

        protected internal UpdateInfo() { }

        internal UpdateInfo(DateTime? updatedOn = null)
        {
            if (updatedOn is null) updatedOn = DateTime.UtcNow;
            UpdatedOn = updatedOn.Value;
        }
    }

    [Owned]
    public class UpdateInfo<TUpdaterKey> : UpdateInfo
    {
        [Column("UpdatedBy")]
        public TUpdaterKey UpdaterId { get; set; }

        protected internal UpdateInfo() { }

        internal UpdateInfo(TUpdaterKey updaterId, DateTime? updatedOn = null) : base(updatedOn)
        {
            ArgumentNullException.ThrowIfNull(updaterId);
            UpdaterId = updaterId;
        }
    }

    [Owned]
    public class UpdateInfo<TUpdater, TUpdaterKey> : UpdateInfo<TUpdaterKey>
        where TUpdater : IIdentifiable<TUpdaterKey>
    {
        [ForeignKey(nameof(UpdaterId))]
        public TUpdater Updater { get; set; }

        protected internal UpdateInfo() { }

        internal UpdateInfo(TUpdaterKey updaterId, DateTime? updatedOn = null) : base(updaterId, updatedOn) { }

        internal UpdateInfo(TUpdater updater, DateTime? updatedOn = null) : this(updater.Id!, updatedOn)
        {
            Updater = updater;
        }
    }
}
