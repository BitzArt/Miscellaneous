namespace BitzArt.XUnit;

public class TransientDependency(Guid globalId)
{
    public Guid GlobalId { get; } = globalId;
    public Guid LocalId { get; } = Guid.NewGuid();
}
