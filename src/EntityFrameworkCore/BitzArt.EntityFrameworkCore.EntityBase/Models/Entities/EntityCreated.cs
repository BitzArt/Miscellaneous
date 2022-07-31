namespace BitzArt.EntityBase;

public abstract class EntityCreated<TKey> : EntityBase<TKey>
{
    public CreationInfo CreationInfo { get; private set; }

    protected internal EntityCreated() { }

    public EntityCreated(DateTime? createdOn = null)
    {
        CreationInfo = new(createdOn);
    }
}

public abstract class EntityCreated<TKey, TCreatorKey> : EntityBase<TKey>
{
    public CreationInfo<TCreatorKey> CreationInfo { get; private set; }

    protected internal EntityCreated() { }

    public EntityCreated(TCreatorKey creatorId, DateTime? createdOn = null)
    {
        CreationInfo = new(creatorId, createdOn);
    }
}

public abstract class EntityCreated<TKey, TCreator, TCreatorKey> : EntityBase<TKey>
    where TCreator : IIdentifiable<TCreatorKey>
{
    public CreationInfo<TCreator, TCreatorKey> CreationInfo { get; private set; }

    protected internal EntityCreated() { }

    public EntityCreated(TCreator creator, DateTime? createdOn = null)
    {
        CreationInfo = new(creator, createdOn);
    }
}
