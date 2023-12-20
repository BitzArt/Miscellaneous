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
}