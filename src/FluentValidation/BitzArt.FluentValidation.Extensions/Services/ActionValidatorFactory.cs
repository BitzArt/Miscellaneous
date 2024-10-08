using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace FluentValidation;

internal class ActionValidatorFactory(IServiceProvider serviceProvider) : IActionValidatorFactory
{
    internal static ConcurrentDictionary<Type, Type> ValidatorTypeMap = [];

    internal IServiceProvider _serviceProvider = serviceProvider;

    private ActionType? _actionType = null;

    public IActionValidator<T> GetValidator<T>(ActionType actionType)
        => (IActionValidator<T>)GetValidatorInternal(typeof(IActionValidator<T>), definedActionType: actionType);

    public IActionValidator GetValidator(Type objectType, ActionType actionType)
        => GetValidatorInternal(typeof(IActionValidator<>).MakeGenericType(objectType), definedActionType: actionType);

    public IActionValidator GetValidatorInternal(Type validatorType, Func<IServiceProvider, ActionType?>? actionTypeResolver = null, ActionType? definedActionType = null)
    {
        bool cleanup = false;
        try
        {
            var implementationType = ValidatorTypeMap[validatorType]
                ?? throw new ArgumentException($"{validatorType.Name} is not registered as ActionValidator");

            if (definedActionType.HasValue)
            {
                _actionType = definedActionType;
                cleanup = true;
            }

            var validator = (IActionValidator)_serviceProvider.GetRequiredService(implementationType);

            if (_actionType.HasValue)
            {
                validator.Action = _actionType!.Value;
                return validator;
            }

            if (actionTypeResolver is not null)
            {
                validator.Action = actionTypeResolver(_serviceProvider);
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
