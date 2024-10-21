using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace FluentValidation;

public static class AddActionValidatorExtensions
{
    public static IServiceCollection AddActionValidatorsFromAssemblyContaining<TAssemblyPointer>(this IServiceCollection services, ActionType actionType)
        => services.AddActionValidatorsFromAssemblyContaining(typeof(TAssemblyPointer), actionType);

    public static IServiceCollection AddActionValidatorsFromAssemblyContaining(this IServiceCollection services, Type type, ActionType actionType)
        => services.AddActionValidatorsFromAssembly(type.Assembly, actionType);

    public static IServiceCollection AddActionValidatorsFromAssemblyContaining<TAssemblyPointer>(this IServiceCollection services, Func<IServiceProvider, ActionType> actionTypeResolver)
        => services.AddActionValidatorsFromAssemblyContaining(typeof(TAssemblyPointer), actionTypeResolver);

    public static IServiceCollection AddActionValidatorsFromAssemblyContaining(this IServiceCollection services, Type type, Func<IServiceProvider, ActionType> actionTypeResolver)
        => services.AddActionValidatorsFromAssembly(type.Assembly, actionTypeResolver);

    public static IServiceCollection AddActionValidatorsFromAssembly(this IServiceCollection services, Assembly assembly, ActionType actionType)
    {
        var validators = assembly
            .DefinedTypes
            .Where(x => x.IsClass && !x.IsAbstract)
            .Where(x => x.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IActionValidator<>)));

        foreach (var validator in validators) services.AddActionValidator(validator, actionType);

        return services;
    }

    public static IServiceCollection AddActionValidatorsFromAssembly(this IServiceCollection services, Assembly assembly, Func<IServiceProvider, ActionType> actionTypeResolver)
    {
        var validators = assembly
            .DefinedTypes
            .Where(x => x.IsClass && !x.IsAbstract)
            .Where(x => x.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IActionValidator<>)));

        foreach (var validator in validators) services.AddActionValidator(validator, actionTypeResolver);

        return services;
    }

    public static IServiceCollection AddActionValidator<TValidator>(this IServiceCollection services, ActionType actionType)
        => services.AddActionValidator(typeof(TValidator), actionType);

    public static IServiceCollection AddActionValidator(this IServiceCollection services, Type validatorType, ActionType actionType)
    {
        if (validatorType is null) throw new ArgumentException($"{nameof(validatorType)} must not be null");

        services.TryAddScoped<IActionValidatorFactory>(serviceProvider => new ActionValidatorFactory(serviceProvider));

        var interfaceDefinitions = validatorType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IActionValidator<>)).ToList();
        if (interfaceDefinitions.Count == 0) throw new ArgumentException($"{validatorType.Name} does not implement IActionValidator<T>");

        services.AddTransient(validatorType);
        var mapEntry = new ValidatorMapEntry(validatorType, DefinedActionType: actionType);
        services.AddKeyedSingleton(serviceKey: validatorType, implementationInstance: mapEntry);

        foreach (var interfaceDefinition in interfaceDefinitions)
        {
            var validationObjectType = interfaceDefinition.GetGenericArguments().First();
            services.AddActionValidator(validatorType, validationObjectType, actionType, mapEntry);
        }

        return services;
    }

    public static IServiceCollection AddActionValidator<TValidator>(this IServiceCollection services, Func<IServiceProvider, ActionType> actionTypeResolver)
        => services.AddActionValidator(typeof(TValidator), actionTypeResolver);

    public static IServiceCollection AddActionValidator(this IServiceCollection services, Type validatorType, Func<IServiceProvider, ActionType> actionTypeResolver)
    {
        if (validatorType is null) throw new ArgumentException($"{nameof(validatorType)} must not be null");

        services.TryAddScoped<IActionValidatorFactory>(serviceProvider => new ActionValidatorFactory(serviceProvider));

        var interfaceDefinitions = validatorType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IActionValidator<>)).ToList();
        if (interfaceDefinitions.Count == 0) throw new ArgumentException($"{validatorType.Name} does not implement IActionValidator<T>");

        services.AddTransient(validatorType);
        var mapEntry = new ValidatorMapEntry(validatorType, ActionTypeResolver: actionTypeResolver);
        services.AddKeyedSingleton(serviceKey: validatorType, implementationInstance: mapEntry);

        foreach (var interfaceDefinition in interfaceDefinitions)
        {
            var validationObjectType = interfaceDefinition.GetGenericArguments().First();
            services.AddActionValidator(validatorType, validationObjectType, actionTypeResolver, mapEntry);
        }

        return services;
    }

    private static IServiceCollection AddActionValidator(this IServiceCollection services, Type validatorType, Type validationObjectType, ActionType actionType, ValidatorMapEntry mapEntry)
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
                var validator = factory.GetValidatorInternal(validatorType);

                return validator;
            });

            services.AddKeyedSingleton(serviceKey: registrationInterface, implementationInstance: mapEntry);
        }

        return services;
    }

    private static IServiceCollection AddActionValidator(this IServiceCollection services, Type validatorType, Type validationObjectType, Func<IServiceProvider, ActionType> getActionType, ValidatorMapEntry mapEntry)
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
                var validator = factory.GetValidatorInternal(validatorType);

                return validator;
            });

            services.AddKeyedSingleton(serviceKey: registrationInterface, implementationInstance: mapEntry);
        }

        return services;
    }
}
