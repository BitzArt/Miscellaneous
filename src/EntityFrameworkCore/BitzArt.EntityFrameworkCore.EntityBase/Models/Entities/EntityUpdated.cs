namespace BitzArt.EntityBase;

public abstract class EntityUpdated<TKey> : EntityCreated<TKey>
    where TKey : struct
{
    public UpdateInfo UpdateInfo { get; private set; }

    protected EntityUpdated() : base() { }

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
    where TKey : struct
    where TUpdaterKey : struct
{
    public UpdateInfo<TUpdaterKey> UpdateInfo { get; private set; }

    protected EntityUpdated() : base() { }

    public EntityUpdated(TUpdaterKey creatorId, DateTime? createdOn = null) : base(creatorId, createdOn)
    {
        Updated(creatorId, createdOn);
    }

    public void Updated(TUpdaterKey updaterId, DateTime? updatedOn = null)
    {
        UpdateInfo = new(updaterId, updatedOn);
    }
}

public abstract class EntityUpdated<TKey, TUpdater, TUpdaterKey> : EntityCreated<TKey, TUpdater, TUpdaterKey>
    where TKey : struct
    where TUpdaterKey : struct
    where TUpdater : IIdentifiable<TUpdaterKey>
{
    public UpdateInfo<TUpdater, TUpdaterKey> UpdateInfo { get; private set; }

    protected EntityUpdated() : base() { }

    public EntityUpdated(TUpdaterKey creatorId, DateTime? createdOn = null) : base(creatorId, createdOn)
    {
        Updated(creatorId, createdOn);
    }

    public EntityUpdated(TUpdater creator, DateTime? createdOn = null) : base(creator, createdOn)
    {
        Updated(creator, createdOn);
    }

    public void Updated(TUpdaterKey updaterId, DateTime? updatedOn = null)
    {
        UpdateInfo = new(updaterId, updatedOn);
    }

    public void Updated(TUpdater updater, DateTime? updatedOn = null)
    {
        UpdateInfo = new(updater, updatedOn);
    }
}
