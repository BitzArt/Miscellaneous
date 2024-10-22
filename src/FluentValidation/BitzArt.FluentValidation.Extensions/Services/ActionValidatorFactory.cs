using Microsoft.Extensions.DependencyInjection;

namespace FluentValidation;

internal class ActionValidatorFactory(IServiceProvider serviceProvider) : IActionValidatorFactory
{
    internal IServiceProvider _serviceProvider = serviceProvider;

    private ActionType? _actionType = null;

    public IActionValidator<T> GetValidator<T>(ActionType? actionType = null)
        => (IActionValidator<T>)GetValidatorInternal(typeof(IActionValidator<T>), actionType);

    public IActionValidator GetValidator(Type objectType, ActionType? actionType = null)
        => GetValidatorInternal(typeof(IActionValidator<>).MakeGenericType(objectType), actionType);

    public IActionValidator GetValidatorInternal(Type validatorType, ActionType? actionTypeOverride = null)
    {
        bool cleanup = false;

        try
        {
            var validatorInfo = _serviceProvider.GetRequiredKeyedService<ValidatorMapEntry>(validatorType);

            if (!_actionType.HasValue)
            {
                if (actionTypeOverride.HasValue)
                {
                    _actionType = actionTypeOverride;
                    cleanup = true;
                }
                else if (validatorInfo.DefinedActionType.HasValue)
                {
                    _actionType = validatorInfo.DefinedActionType!.Value;
                    cleanup = true;
                }
            }

            var validator = (IActionValidator)_serviceProvider.GetRequiredService(validatorInfo.ImplementationType);

            if (_actionType.HasValue)
            {
                validator.Action = _actionType!.Value;
                return validator;
            }

            if (validatorInfo.ActionTypeResolver is not null)
            {
                validator.Action = validatorInfo.ActionTypeResolver.Invoke(_serviceProvider);
                return validator;
            }

            return validator;
        }
        finally
        {
            if (cleanup) _actionType = null;
        }
    }
}
