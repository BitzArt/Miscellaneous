namespace FluentValidation;

internal class TestOptions(ActionType actionType)
{
    public ActionType ActionType { get; set; } = actionType;
}
