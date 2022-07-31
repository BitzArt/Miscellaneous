namespace BitzArt.EntityBase;

public abstract class EntityUpdated<TKey> : EntityCreated<TKey>
{
    public UpdateInfo UpdateInfo { get; private set; }

    protected internal EntityUpdated() { }

    public EntityUpdated(DateTime? createdOn = null) : base(createdOn)
    {
        Updated(createdOn);
    }

    public void Updated(DateTime? updatedOn = null)
    {
        UpdateInfo = new(updatedOn);
    }
}

public abstract class EntityUpdated<TKey, TUpdaterKey> : EntityCreated<TKey, TUpdaterKey>
{
    public UpdateInfo<TUpdaterKey> UpdateInfo { get; private set; }

    protected internal EntityUpdated() { }

    public EntityUpdated(TUpdaterKey updaterId, DateTime? createdOn = null) : base(updaterId, createdOn)
    {
        Updated(updaterId, createdOn);
    }

    public void Updated(TUpdaterKey updaterId, DateTime? updatedOn = null)
    {
        UpdateInfo = new(updaterId, updatedOn);
    }
}

public abstract class EntityUpdated<TKey, TUpdater, TUpdaterKey> : EntityCreated<TKey, TUpdater, TUpdaterKey>
    where TUpdater : IIdentifiable<TUpdaterKey>
{
    public UpdateInfo<TUpdater, TUpdaterKey> UpdateInfo { get; private set; }

    protected internal EntityUpdated() { }

    public EntityUpdated(TUpdater updater, DateTime? updatedOn = null) : base(updater, updatedOn)
    {
        Updated(updater, updatedOn);
    }

    public void Updated(TUpdater updater, DateTime? updatedOn = null)
    {
        UpdateInfo = new(updater, updatedOn);
    }
}
