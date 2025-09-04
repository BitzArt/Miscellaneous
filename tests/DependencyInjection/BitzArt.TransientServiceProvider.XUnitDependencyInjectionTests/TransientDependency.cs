namespace BitzArt.XUnit;

public class TransientDependency(Guid globalId, Guid localId)
{
    public Guid GlobalId { get; } = globalId;
    public Guid LocalId { get; } = localId;
}
