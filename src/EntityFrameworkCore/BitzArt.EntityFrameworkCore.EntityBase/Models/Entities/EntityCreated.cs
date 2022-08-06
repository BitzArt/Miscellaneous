namespace BitzArt.EntityBase;

public abstract class EntityCreated<TKey> : EntityBase<TKey>
    where TKey : struct
{
    public CreationInfo CreationInfo { get; private set; }

    protected EntityCreated()
    {
        CreationInfo = new();
    }

    public EntityCreated(DateTime? createdOn = null)
    {
        CreationInfo = new(createdOn);
    }
}

public abstract class EntityCreated : EntityCreated<Guid> { }

public abstract class EntityCreated<TKey, TCreatorKey> : EntityBase<TKey>
    where TKey : struct
    where TCreatorKey : struct
{
    public CreationInfo<TCreatorKey> CreationInfo { get; private set; }

    protected EntityCreated()
    {
        CreationInfo = new();
    }

    public EntityCreated(TCreatorKey creatorId, DateTime? createdOn = null)
    {
        CreationInfo = new(creatorId, createdOn);
    }
}

public abstract class EntityCreated<TKey, TCreator, TCreatorKey> : EntityBase<TKey>
    where TKey : struct
    where TCreatorKey : struct
    where TCreator : IIdentifiable<TCreatorKey>
{
    public CreationInfo<TCreator, TCreatorKey> CreationInfo { get; private set; }

    protected EntityCreated()
    {
        CreationInfo = new();
    }

    public EntityCreated(TCreatorKey creatorId, DateTime? createdOn = null)
    {
        CreationInfo = new(creatorId, createdOn);
    }

    public EntityCreated(TCreator creator, DateTime? createdOn = null)
    {
        CreationInfo = new(creator, createdOn);
    }
}
