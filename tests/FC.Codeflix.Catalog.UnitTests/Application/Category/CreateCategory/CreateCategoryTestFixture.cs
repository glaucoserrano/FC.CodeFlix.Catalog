using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> { }
public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
{

    public CreateCategoryInput GetInput()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );
    public CreateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetInput();
        invalidInputShortName.Name =
            invalidInputShortName.Name.Substring(0, 2);
        return invalidInputShortName;
    }

    public CreateCategoryInput GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = GetInput();
        var tooLongNameCategory = Faker.Commerce.ProductName();
        while (tooLongNameCategory.Length <= 255)
            tooLongNameCategory = $"{tooLongNameCategory} {Faker.Commerce.ProductName()}";

        invalidInputTooLongName.Name =
            tooLongNameCategory;

        return invalidInputTooLongName;

    }
    public CreateCategoryInput GetInvalidInputDescriptionNull()
    {
        var invalidInputDescriptionNull = GetInput();

        invalidInputDescriptionNull.Description = null!;

        return invalidInputDescriptionNull;
    }

    public CreateCategoryInput GetInvalidInputTooLongDescription()
    {
        var invalidInputTooLongDescription = GetInput();
        var tooLongDescriptionCategory = Faker.Commerce.ProductDescription();
        while (tooLongDescriptionCategory.Length <= 10_000)
            tooLongDescriptionCategory = $"{tooLongDescriptionCategory} {Faker.Commerce.ProductDescription()}";

        invalidInputTooLongDescription.Description =
            tooLongDescriptionCategory;

        return invalidInputTooLongDescription;

    }
}
