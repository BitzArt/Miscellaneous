using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FluentValidation;

public static class AddActionValidatorsExtension
{
    public static IServiceCollection AddActionValidatorsFromAssemblyContaining<TAssemblyPointer>(this IServiceCollection services, Func<IServiceProvider, ActionType> getActionType)
        => services.AddActionValidatorsFromAssemblyContaining(typeof(TAssemblyPointer), getActionType);

    public static IServiceCollection AddActionValidatorsFromAssemblyContaining(this IServiceCollection services, Type type, Func<IServiceProvider, ActionType> getActionType)
        => services.AddActionValidatorsFromAssembly(type.Assembly, getActionType);

    public static IServiceCollection AddActionValidatorsFromAssembly(this IServiceCollection services, Assembly assembly, Func<IServiceProvider, ActionType> getActionType)
    {
        var validators = assembly
            .DefinedTypes
            .Where(x => x.BaseType is not null && x.BaseType.IsGenericType)
            .Where(x => x.BaseType!.GetGenericTypeDefinition() == typeof(ActionValidator<>));

        foreach (var validator in validators) services.AddActionValidator(validator, getActionType);

        return services;
    }

    public static IServiceCollection AddActionValidator(this IServiceCollection services, Type validatorType, Func<IServiceProvider, ActionType> getActionType)
    {
        if (validatorType is null) throw new ArgumentException($"{nameof(validatorType)} must not be null");
        if (validatorType.BaseType!.GetGenericTypeDefinition() != typeof(ActionValidator<>)) throw new ArgumentException($"{validatorType.Name} is not assignable to ActionValidator");

        var baseClassDefinition = validatorType.BaseType!;
        var validationObjectType = baseClassDefinition.GetGenericArguments().Single();

        var registrationType = typeof(IValidator<>).MakeGenericType(validationObjectType);

        services.AddTransient(validatorType);
        services.AddScoped(registrationType, x =>
        {
            var validator = x.GetRequiredService(validatorType);
            (validator as IActionValidator)!.ActionType = getActionType.Invoke(x);
            return validator;
        });

        return services;
    }
}
