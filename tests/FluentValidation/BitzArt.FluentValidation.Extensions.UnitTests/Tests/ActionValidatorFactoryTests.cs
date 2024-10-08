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
        services.AddActionValidator(validatorType);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IActionValidatorFactory>();

        var validator = factory.GetValidatorInternal(validatorType, (_) => null, actionType: actionType);

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
    public void GetValidatorReflexion_OnDefinedActionValidator_ShouldReturnValidator(ActionType actionType)
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);

        // Act
        services.AddActionValidator(validatorType);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IActionValidatorFactory>();

        var validator = factory.GetValidator(typeof(TestEntity), actionType);

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
    public void GetValidatorGeneric_OnDefinedActionValidator_ShouldReturnValidator(ActionType actionType)
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();
        var validatorType = typeof(TestEntityValidator);

        // Act
        services.AddActionValidator(validatorType);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IActionValidatorFactory>();

        var validator = factory.GetValidator<TestEntity>(actionType);

        Assert.NotNull(validator);
        Assert.True(validator is TestEntityValidator);
        Assert.Equal(actionType, ((TestEntityValidator)validator).Action);
    }
}
