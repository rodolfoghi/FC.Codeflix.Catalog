using FC.Codeflix.Catalog.Domain.Exceptions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validData = new
        {
            Name = "Category name",
            Description = "Category description",
        };
        var datetimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotEqual(default, category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWhitIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWhitIsActive(bool isActive)
    {
        var validData = new
        {
            Name = "Category name",
            Description = "Category description",
        };
        var datetimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotEqual(default, category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        Action action = () => new DomainEntity.Category(name!, "Category description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        Action action = () => new DomainEntity.Category("Category name", description: null!);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ab")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        Action action = () => new DomainEntity.Category(invalidName, "Category description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal($"Name should be at least {DomainEntity.Category.NameMinLength} characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        string invalidName = new string('a', DomainEntity.Category.NameMaxLength + 1);
        Action action = () => new DomainEntity.Category(invalidName, "Category description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal($"Name should be at greater than {DomainEntity.Category.NameMaxLength} characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        string invalidDescription = new string('a', DomainEntity.Category.DescriptionMaxLength + 1);
        Action action = () => new DomainEntity.Category("Category name", invalidDescription);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal($"Description should be at greater than {DomainEntity.Category.DescriptionMaxLength} characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        var validData = new
        {
            Name = "Category name",
            Description = "Category description",
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive: false);
        Assert.False(category.IsActive);
        category.Activate();

        Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validData = new
        {
            Name = "Category name",
            Description = "Category description",
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive: true);
        Assert.True(category.IsActive);
        category.Deactivate();

        Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var category = new DomainEntity.Category("Category name", "Category description");
        var newValues = new { Name = "New name", Description = "New description" };

        category.Update(newValues.Name, newValues.Description);

        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(newValues.Description, category.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = new DomainEntity.Category("Category name", "Category description");
        var newValues = new { Name = "New name" };
        var currentDescription = category.Description;

        category.Update(newValues.Name);

        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(currentDescription, category.Description);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = new DomainEntity.Category("Category name", "Category description");

        Action action = () => category.Update(name!);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsNull()
    {
        var category = new DomainEntity.Category("Category name", "Category description");

        Action action = () => category.Update("Category name", description: null!);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be null", exception.Message);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ab")]
    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var category = new DomainEntity.Category("Category name", "Category description");

        Action action = () => category.Update(invalidName);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal($"Name should be at least {DomainEntity.Category.NameMinLength} characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var category = new DomainEntity.Category("Category name", "Category description");
        string invalidName = new string('a', DomainEntity.Category.NameMaxLength + 1);
        Action action = () => category.Update(invalidName);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal($"Name should be at greater than {DomainEntity.Category.NameMaxLength} characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var category = new DomainEntity.Category("Category name", "Category description");
        string invalidDescription = new string('a', DomainEntity.Category.DescriptionMaxLength + 1);

        Action action = () => category.Update(category.Name, invalidDescription);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal($"Description should be at greater than {DomainEntity.Category.DescriptionMaxLength} characters long", exception.Message);
    }
}