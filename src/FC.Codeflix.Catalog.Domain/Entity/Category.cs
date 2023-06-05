using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.Domain.Entity;

public class Category
{
    public const int NameMinLength = 3;

    public const int NameMaxLength = 255;

    public const int DescriptionMaxLength = 10_000;

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Category(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = true;
        CreatedAt = DateTime.Now;
        Validate();
    }

    public Category(string name, string description, bool isActive) : this(name, description)
    {
        IsActive = isActive;
        Validate();
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new EntityValidationException($"{nameof(Name)} should not be empty or null");

        if (Name.Length < NameMinLength)
            throw new EntityValidationException($"{nameof(Name)} should be at least {NameMinLength} characters long");

        if (Name.Length > NameMaxLength)
            throw new EntityValidationException($"{nameof(Name)} should be at greater than {NameMaxLength} characters long");

        if (Description is null)
            throw new EntityValidationException($"{nameof(Description)} should not be null");

        if (Description.Length > DescriptionMaxLength)
            throw new EntityValidationException($"{nameof(Description)} should be at greater than {DescriptionMaxLength} characters long");
    }
}
