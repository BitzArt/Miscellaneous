using BitzArt.EnumToMemberValue;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace FluentValidation;

public static class AddActionValidatorsExtension
{
    public static IServiceCollection AddActionValidatorsFromAssemblyContaining<TAssemblyPointer>(this IServiceCollection services, Func<IServiceProvider, ActionType>? getActionType = null)
        => services.AddActionValidatorsFromAssemblyContaining(typeof(TAssemblyPointer), getActionType);

    public static IServiceCollection AddActionValidatorsFromAssemblyContaining(this IServiceCollection services, Type type, Func<IServiceProvider, ActionType>? getActionType = null)
        => services.AddActionValidatorsFromAssembly(type.Assembly, getActionType);

    public static IServiceCollection AddActionValidatorsFromAssembly(this IServiceCollection services, Assembly assembly, Func<IServiceProvider, ActionType>? getActionType = null)
    {
        var validators = assembly
            .DefinedTypes
            .Where(x => x.BaseType is not null && x.BaseType.IsGenericType)
            .Where(x => x.BaseType!.GetGenericTypeDefinition() == typeof(ActionValidator<>));

        foreach (var validator in validators) services.AddActionValidator(validator, getActionType);

        return services;
    }

    public static IServiceCollection AddActionValidator(this IServiceCollection services, Type validatorType, Func<IServiceProvider, ActionType>? getActionType = null)
    {
        if (validatorType is null) throw new ArgumentException($"{nameof(validatorType)} must not be null");
        if (validatorType.BaseType!.GetGenericTypeDefinition() != typeof(ActionValidator<>)) throw new ArgumentException($"{validatorType.Name} is not assignable to ActionValidator");

        var baseClassDefinition = validatorType.BaseType!;
        var validationObjectType = baseClassDefinition.GetGenericArguments().Single();

        var registrationType = typeof(IValidator<>).MakeGenericType(validationObjectType);

        services.AddTransient(validatorType);

        if (getActionType is not null) services.AddScoped(registrationType, x =>
        {
            var validator = x.GetRequiredService(validatorType);
            (validator as IActionValidator)!.ActionType = getActionType.Invoke(x);
            return validator;
        });

        services.AddKeyedForEnum(
            registrationType,
            x => (IActionValidator)x.GetRequiredService(validatorType),
            (ActionType type) => type.ToMemberValue(),
            (validator, key) => validator.ActionType = key,
            ServiceLifetime.Scoped);

        return services;
    }

    private static void AddKeyedForEnum<TService, TEnum>(
        this IServiceCollection services,
        Type registrationType,
        Func<IServiceProvider, TService> implementationFactory,
        Func<TEnum, string> enumStringValueFactory,
        Action<TService, TEnum> applyKeyAction,
        ServiceLifetime serviceLifetime)
        where TService : class
        where TEnum : struct, Enum
    {
        var enumValues = Enum.GetValues<TEnum>();

        foreach (var enumValue in enumValues)
        {
            services.Add(
                new ServiceDescriptor(
                    registrationType,
                    enumValue,
                    (x, key) =>
                    {
                        var service = implementationFactory(x);
                        applyKeyAction(service, enumValue);
                        return service;
                    },
                    serviceLifetime));

            services.Add(
                new ServiceDescriptor(
                    registrationType,
                    enumStringValueFactory(enumValue),
                    (x, key) =>
                    {
                        var service = implementationFactory(x);
                        applyKeyAction(service, enumValue);
                        return service;
                    },
                    serviceLifetime));
        }
    }
}
