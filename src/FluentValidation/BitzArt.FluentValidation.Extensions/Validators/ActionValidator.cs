namespace FluentValidation;

public abstract class ActionValidator<T> : JsonValidator<T>
{
    protected readonly ActionType ActionType;

    protected ActionValidator(ActionType actionType)
    {
        ActionType = actionType;
    }
    public IConditionBuilder When(ActionType actionType, Action action)
        => When(x => ActionType == actionType, action);

    public IConditionBuilder Unless(ActionType actionType, Action action)
        => Unless(x => ActionType == actionType, action);
}

public enum ActionType : byte
{
    Get = 1,
    Create = 2,
    Update = 3,
    Patch = 4,
    Options = 5,
    Delete = 6
}
