
namespace FluentValidation;

public interface IActionValidatorFactory
{
    public IActionValidator<T> GetValidator<T>(ActionType actionType);

    public IActionValidator GetValidator(Type objectType, ActionType actionType);

    internal IActionValidator GetValidatorInternal(Type validatorType, Func<IServiceProvider, ActionType?> getActionType, ActionType? actionType = null);
}
