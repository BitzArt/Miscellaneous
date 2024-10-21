
namespace FluentValidation;

public interface IActionValidatorFactory
{
    public IActionValidator<T> GetValidator<T>(ActionType? actionType = null);

    public IActionValidator GetValidator(Type objectType, ActionType? actionType = null);

    internal IActionValidator GetValidatorInternal(Type validatorType, ActionType? actionType = null);
}
