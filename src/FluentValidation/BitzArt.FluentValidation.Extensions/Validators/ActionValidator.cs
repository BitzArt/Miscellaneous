namespace FluentValidation;

public abstract class ActionValidator<T> : AbstractValidator<T>, IActionValidator
{
    private ActionType? _actionType;
    public ActionType ActionType
    {
        get => _actionType is not null ?
            _actionType!.Value :
            throw new ArgumentException("ActionType is not configured for this ActionValidator.");

        set => _actionType = value;
    }

    public IConditionBuilder When(ActionType actionType, Action action)
        => When(x => ActionType == actionType, action);

    public IConditionBuilder Unless(ActionType actionType, Action action)
        => Unless(x => ActionType == actionType, action);
}
