namespace FluentValidation;

public abstract class ActionValidator<T> : AbstractValidator<T>, IActionValidator<T>
{
    public ActionType? Action { get; set; }

    public IConditionBuilder When(ActionType actionType, Action action)
        => When(x => Action == actionType, action);

    public IConditionBuilder Unless(ActionType actionType, Action action)
        => Unless(x => Action == actionType, action);
}
