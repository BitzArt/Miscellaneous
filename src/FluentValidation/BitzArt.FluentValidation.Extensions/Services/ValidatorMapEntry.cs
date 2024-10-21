namespace FluentValidation;

internal record ValidatorMapEntry
{
    public Type ImplementationType { get; }
    public Func<IServiceProvider, ActionType>? ActionTypeResolver { get; }
    public ActionType? DefinedActionType { get; }

    public ValidatorMapEntry(Type ImplementationType, Func<IServiceProvider, ActionType>? ActionTypeResolver = null, ActionType? DefinedActionType = null)
    {
        ArgumentNullException.ThrowIfNull(ImplementationType, nameof(ImplementationType));
        if (ActionTypeResolver is null && DefinedActionType is null) throw new ArgumentException("Either ActionTypeResolver or DefinedActionType must be provided");

        this.ImplementationType = ImplementationType;
        this.ActionTypeResolver = ActionTypeResolver;
        this.DefinedActionType = DefinedActionType;
    }
}
