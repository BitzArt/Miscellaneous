namespace FluentValidation;

public interface IActionValidator<T> : IValidator<T>, IActionValidator
{

}

public interface IActionValidator
{
    public ActionType? Action { get; internal set; }
}
