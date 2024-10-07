using BitzArt.EnumToMemberValue;
using FluentValidation.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FluentValidation;

public class AddActionValidatorsExtensionTests
{
    [Theory]
    [InlineData(ActionType.Get)]
    [InlineData(ActionType.Create)]
    [InlineData(ActionType.Patch)]
    [InlineData(ActionType.Update)]
    public void AddActionValidator_WithFlatActionType_AddsValidator(ActionType actionType)
    {
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);

        services.AddActionValidator(validatorType, x => actionType);

        var serviceProvider = services.BuildServiceProvider();

        var validator = serviceProvider.GetService<IValidator<TestEntity>>();

        Assert.NotNull(validator);
        Assert.True(validator is TestEntityValidator);
        var validatorCasted = (TestEntityValidator)validator;
        Assert.Equal(actionType, validatorCasted.ActionType);
    }

    [Theory]
    [InlineData(ActionType.Get)]
    [InlineData(ActionType.Create)]
    [InlineData(ActionType.Patch)]
    [InlineData(ActionType.Update)]
    public void AddActionValidator_WithActionTypeFromDi_AddsValidator(ActionType actionType)
    {
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);

        var options = new TestOptions(actionType);
        services.AddSingleton(options);
        services.AddActionValidator(validatorType, x => x.GetRequiredService<TestOptions>().ActionType);

        var serviceProvider = services.BuildServiceProvider();

        var validator = serviceProvider.GetService<IValidator<TestEntity>>();

        Assert.NotNull(validator);
        Assert.True(validator is TestEntityValidator);
        var validatorCasted = (TestEntityValidator)validator;
        Assert.Equal(actionType, validatorCasted.ActionType);
    }

    [Fact]
    public void AddActionValidatorsFromAssembly_WithFlatActionType_AddsTestEntityValidator()
    {
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);
        var actionType = ActionType.Get;

        services.AddActionValidatorsFromAssembly(typeof(AddActionValidatorsExtensionTests).Assembly, x => actionType);

        var serviceProvider = services.BuildServiceProvider();

        var validator = serviceProvider.GetService<IValidator<TestEntity>>();

        Assert.NotNull(validator);
        Assert.True(validator is TestEntityValidator);
        var validatorCasted = (TestEntityValidator)validator;
        Assert.Equal(actionType, validatorCasted.ActionType);
    }

    [Fact]
    public void AddActionValidatorsFromAssemblyContaining_WithFlatActionType_AddsTestEntityValidator()
    {
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);
        var actionType = ActionType.Get;

        services.AddActionValidatorsFromAssemblyContaining(typeof(AddActionValidatorsExtensionTests), x => actionType);

        var serviceProvider = services.BuildServiceProvider();

        var validator = serviceProvider.GetService<IValidator<TestEntity>>();

        Assert.NotNull(validator);
        Assert.True(validator is TestEntityValidator);
        var validatorCasted = (TestEntityValidator)validator;
        Assert.Equal(actionType, validatorCasted.ActionType);
    }

    [Fact]
    public void AddActionValidatorsFromAssemblyContainingGeneric_WithFlatActionType_AddsTestEntityValidator()
    {
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);
        var actionType = ActionType.Get;

        services.AddActionValidatorsFromAssemblyContaining<AddActionValidatorsExtensionTests>(x => actionType);

        var serviceProvider = services.BuildServiceProvider();

        var validator = serviceProvider.GetService<IValidator<TestEntity>>();

        Assert.NotNull(validator);
        Assert.True(validator is TestEntityValidator);
        var validatorCasted = (TestEntityValidator)validator;
        Assert.Equal(actionType, validatorCasted.ActionType);
    }

    [Theory]
    [InlineData(ActionType.Get)]
    [InlineData(ActionType.Create)]
    [InlineData(ActionType.Update)]
    [InlineData(ActionType.Patch)]
    [InlineData(ActionType.Options)]
    [InlineData(ActionType.Delete)]
    public void AddActionValidator_WithoutSpecifyingActionType_ValidatorAccessibleByActionType(ActionType resolveKey)
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);

        // Act
        services.AddActionValidator(validatorType);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var validator = serviceProvider.GetRequiredKeyedService<IValidator<TestEntity>>(resolveKey);

        Assert.NotNull(validator);
        Assert.True(validator is TestEntityValidator);
        Assert.Equal(resolveKey, ((TestEntityValidator)validator).ActionType);
    }

    [Theory]
    [InlineData(ActionType.Get)]
    [InlineData(ActionType.Create)]
    [InlineData(ActionType.Update)]
    [InlineData(ActionType.Patch)]
    [InlineData(ActionType.Options)]
    [InlineData(ActionType.Delete)]
    public void AddActionValidator_WithActionType_ValidatorStillAccessibleByActionType(ActionType resolveKey)
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);

        // Act
        services.AddActionValidator(validatorType, x => default);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var validator = serviceProvider.GetRequiredKeyedService<IValidator<TestEntity>>(resolveKey);

        Assert.NotNull(validator);
        Assert.True(validator is TestEntityValidator);
        Assert.Equal(resolveKey, ((TestEntityValidator)validator).ActionType);
    }

    [Theory]
    [InlineData(ActionTypes.Get)]
    [InlineData(ActionTypes.Create)]
    [InlineData(ActionTypes.Update)]
    [InlineData(ActionTypes.Patch)]
    [InlineData(ActionTypes.Options)]
    [InlineData(ActionTypes.Delete)]
    public void AddActionValidator_WithoutSpecifyingActionType_ValidatorAccessibleByActionTypeEnumMemberValue(string resolveKey)
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);

        // Act
        services.AddActionValidator(validatorType);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var validator = serviceProvider.GetRequiredKeyedService<IValidator<TestEntity>>(resolveKey);

        var expectedActionType = resolveKey.ToEnum<ActionType>();

        Assert.NotNull(validator);
        Assert.True(validator is TestEntityValidator);
        Assert.Equal(expectedActionType, ((TestEntityValidator)validator).ActionType);
    }
}