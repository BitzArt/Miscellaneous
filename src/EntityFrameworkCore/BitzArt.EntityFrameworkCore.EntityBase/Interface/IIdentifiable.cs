namespace BitzArt.EntityBase
{
    public interface IIdentifiable<TKey>
        where TKey : struct
    {
        TKey? Id { get; set; }
    }
}
