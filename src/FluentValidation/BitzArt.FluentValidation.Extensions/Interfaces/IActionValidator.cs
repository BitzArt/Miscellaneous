namespace FluentValidation;

/// <inheritdoc cref="IActionValidator"/>
/// <typeparam name="T">The type of object being validated.</typeparam>
public interface IActionValidator<T> : IValidator<T>, IActionValidator
{

}

/// <summary>
/// An <see cref="IValidator"/> that can be used to validate an object based on the action being performed.
/// </summary>
public interface IActionValidator
{
    /// <summary>
    /// Current action type.
    /// </summary>
    public ActionType? Action { get; internal set; }
}
