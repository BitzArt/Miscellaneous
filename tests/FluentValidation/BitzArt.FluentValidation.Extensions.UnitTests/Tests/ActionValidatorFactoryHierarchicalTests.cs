using Microsoft.Extensions.DependencyInjection;

namespace FluentValidation;

public class ActionValidatorFactoryHierarchicalTests
{
    private class TestHierarchyParent
    {
        public string? Name { get; set; }

        public string? Description { get; set; }
    }

    private class TestHierarchyParentValidator : ActionValidator<TestHierarchyParent>
    {
        public readonly IActionValidator<string> DescriptionValidator;

        public TestHierarchyParentValidator(IActionValidator<string> descriptionValidator)
        {
            DescriptionValidator = descriptionValidator;
            RuleFor(x => x.Description!).SetValidator(DescriptionValidator);
        }
    }

    private class TestDescriptionValidator : ActionValidator<string>
    {
        public TestDescriptionValidator()
        {
            When(ActionType.Create, () =>
            {
                RuleFor(x => x).NotEmpty().MinimumLength(5);
            });
        }
    }

    [Fact]
    public void GetValidator_OnValidatorHierarchy_ShouldSetActionTypeForAllValidators()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddActionValidator<TestHierarchyParentValidator>();
        services.AddActionValidator<TestDescriptionValidator>();


        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IActionValidatorFactory>();

        // Act
        var validator = factory.GetValidator<TestHierarchyParent>(ActionType.Create);

        // Assert
        Assert.NotNull(validator);
        Assert.True(validator is TestHierarchyParentValidator);
        Assert.Equal(ActionType.Create, ((TestHierarchyParentValidator)validator).Action);
        var descriptionValidator = ((TestHierarchyParentValidator)validator).DescriptionValidator;
        Assert.NotNull(descriptionValidator);
        Assert.True(descriptionValidator is TestDescriptionValidator);
        Assert.Equal(ActionType.Create, descriptionValidator.Action);
    }
}
