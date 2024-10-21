using FluentValidation.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FluentValidation;

public class ActionValidatorFactoryTests
{
    [Theory]
    [InlineData(ActionType.Get)]
    [InlineData(ActionType.Create)]
    [InlineData(ActionType.Update)]
    [InlineData(ActionType.Patch)]
    [InlineData(ActionType.Options)]
    [InlineData(ActionType.Delete)]
    public void GetValidatorInternal_OnDefinedActionValidator_ShouldReturnValidator(ActionType actionType)
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);

        // Act
        services.AddActionValidator(validatorType, actionType);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IActionValidatorFactory>();

        var validator = factory.GetValidatorInternal(validatorType);

        Assert.NotNull(validator);
        Assert.True(validator is TestEntityValidator);
        Assert.Equal(actionType, ((TestEntityValidator)validator).Action);
    }

    [Theory]
    [InlineData(ActionType.Get)]
    [InlineData(ActionType.Create)]
    [InlineData(ActionType.Update)]
    [InlineData(ActionType.Patch)]
    [InlineData(ActionType.Options)]
    [InlineData(ActionType.Delete)]
    public void GetValidatorReflection_OnDefinedActionValidator_ShouldReturnValidator(ActionType actionType)
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);

        // Act
        services.AddActionValidator(validatorType, actionType);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IActionValidatorFactory>();

        var validator = factory.GetValidator(typeof(TestEntity));

        Assert.NotNull(validator);
        Assert.True(validator is TestEntityValidator);
        Assert.Equal(actionType, ((TestEntityValidator)validator).Action);
    }

    [Theory]
    [InlineData(ActionType.Get)]
    [InlineData(ActionType.Create)]
    [InlineData(ActionType.Update)]
    [InlineData(ActionType.Patch)]
    [InlineData(ActionType.Options)]
    [InlineData(ActionType.Delete)]
    public void GetValidatorReflection_WithActionTypeOverride_ShouldOverrideActionType(ActionType actionTypeOverride)
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);

        // Act
        services.AddActionValidator(validatorType, 0);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IActionValidatorFactory>();

        var validator = factory.GetValidator(typeof(TestEntity), actionTypeOverride);

        Assert.NotNull(validator);
        Assert.True(validator is TestEntityValidator);
        Assert.NotEqual((byte)0, (byte)((TestEntityValidator)validator).Action!.Value);
        Assert.Equal(actionTypeOverride, ((TestEntityValidator)validator).Action);
    }

    [Theory]
    [InlineData(ActionType.Get)]
    [InlineData(ActionType.Create)]
    [InlineData(ActionType.Update)]
    [InlineData(ActionType.Patch)]
    [InlineData(ActionType.Options)]
    [InlineData(ActionType.Delete)]
    public void GetValidatorGeneric_OnDefinedActionValidator_ShouldReturnValidator(ActionType actionType)
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);

        // Act
        services.AddActionValidator(validatorType, actionType);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IActionValidatorFactory>();

        var validator = factory.GetValidator<TestEntity>(actionType);

        Assert.NotNull(validator);
        Assert.True(validator is TestEntityValidator);
        Assert.Equal(actionType, ((TestEntityValidator)validator).Action);
    }

    [Theory]
    [InlineData(ActionType.Get)]
    [InlineData(ActionType.Create)]
    [InlineData(ActionType.Update)]
    [InlineData(ActionType.Patch)]
    [InlineData(ActionType.Options)]
    [InlineData(ActionType.Delete)]
    public void GetValidatorGeneric_WithActionTypeOverride_ShouldOverrideActionType(ActionType actionTypeOverride)
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);

        // Act
        services.AddActionValidator(validatorType, 0);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IActionValidatorFactory>();

        var validator = factory.GetValidator<TestEntity>(actionTypeOverride);

        Assert.NotNull(validator);
        Assert.True(validator is TestEntityValidator);
        Assert.NotEqual((byte)0, (byte)((TestEntityValidator)validator).Action!.Value);
        Assert.Equal(actionTypeOverride, ((TestEntityValidator)validator).Action);
    }
}
