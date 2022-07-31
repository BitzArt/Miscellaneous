namespace BitzArt.EntityBase
{
    public interface IIdentifiable<TKey>
    {
        TKey? Id { get; set; }
    }
}
