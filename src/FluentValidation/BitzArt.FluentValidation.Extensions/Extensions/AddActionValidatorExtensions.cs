using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace FluentValidation;

public static class AddActionValidatorExtensions
{
    public static IServiceCollection AddActionValidatorsFromAssemblyContaining<TAssemblyPointer>(this IServiceCollection services, Func<IServiceProvider, ActionType>? actionTypeResolver = null)
        => services.AddActionValidatorsFromAssemblyContaining(typeof(TAssemblyPointer), actionTypeResolver);

    public static IServiceCollection AddActionValidatorsFromAssemblyContaining(this IServiceCollection services, Type type, Func<IServiceProvider, ActionType>? actionTypeResolver = null)
        => services.AddActionValidatorsFromAssembly(type.Assembly, actionTypeResolver);

    public static IServiceCollection AddActionValidatorsFromAssembly(this IServiceCollection services, Assembly assembly, Func<IServiceProvider, ActionType>? actionTypeResolver = null)
    {
        var validators = assembly
            .DefinedTypes
            .Where(x => x.IsClass && !x.IsAbstract)
            .Where(x => x.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IActionValidator<>)));

        foreach (var validator in validators) services.AddActionValidator(validator, actionTypeResolver);

        return services;
    }

    public static IServiceCollection AddActionValidator<TValidator>(this IServiceCollection services, Func<IServiceProvider, ActionType>? actionTypeResolver = null)
        => services.AddActionValidator(typeof(TValidator), actionTypeResolver);

    public static IServiceCollection AddActionValidator(this IServiceCollection services, Type validatorType, Func<IServiceProvider, ActionType>? actionTypeResolver = null)
    {
        if (validatorType is null) throw new ArgumentException($"{nameof(validatorType)} must not be null");
        if (validatorType.BaseType!.GetGenericTypeDefinition() != typeof(ActionValidator<>)) throw new ArgumentException($"{validatorType.Name} is not assignable to ActionValidator");

        services.TryAddScoped<IActionValidatorFactory>(serviceProvider => new ActionValidatorFactory(serviceProvider));

        var interfaceDefinitions = validatorType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IActionValidator<>)).ToList();
        if (interfaceDefinitions.Count == 0) throw new ArgumentException($"{validatorType.Name} does not implement IActionValidator<T>");

        services.AddTransient(validatorType);
        ActionValidatorFactory.ValidatorTypeMap[validatorType] = validatorType;

        Func<IServiceProvider, ActionType?> finalActionTypeResolver = actionTypeResolver is not null ?
            x => actionTypeResolver(x) :
            x => null;

        foreach (var interfaceDefinition in interfaceDefinitions)
        {
            var validationObjectType = interfaceDefinition.GetGenericArguments().First();
            services.AddActionValidator(validatorType, validationObjectType, finalActionTypeResolver);
        }

        return services;
    }

    private static IServiceCollection AddActionValidator(this IServiceCollection services, Type validatorType, Type validationObjectType, Func<IServiceProvider, ActionType?> getActionType)
    {
        List<Type> registrationInterfaces =
            [
            typeof(IValidator<>).MakeGenericType(validationObjectType),
            typeof(IActionValidator<>).MakeGenericType(validationObjectType)
            ];

        foreach (var registrationInterface in registrationInterfaces)
        {
            services.AddScoped(registrationInterface, x =>
            {
                var factory = x.GetRequiredService<IActionValidatorFactory>();
                var validator = factory.GetValidatorInternal(validatorType, getActionType: getActionType);

                return validator;
            });

            ActionValidatorFactory.ValidatorTypeMap[registrationInterface] = validatorType;
        }

        return services;
    }
}
