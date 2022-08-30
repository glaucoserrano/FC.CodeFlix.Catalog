using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory;
[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public  class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture> { }
public class UpdateCategoryTestFixture : BaseFixture
{
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<IUnitOfWork> GetUniOfWorkMock() => new();

    public string GetValidCategoryName()
    {
        var categoryName = "";
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];
        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }
    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];

        return categoryDescription;
    }
    public bool GetRandomBoolean() => (new Random()).NextDouble() < 0.5;

    public Category GetExampleCategory()
     => new Category(
         GetValidCategoryName(),
         GetValidCategoryDescription(),
         GetRandomBoolean()
         );
    public UpdateCategoryInput GetValidInput(Guid? id = null) 
        => new(
                id ?? Guid.NewGuid(),
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean()
            );

    public UpdateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetValidInput();
        invalidInputShortName.Name =
            invalidInputShortName.Name.Substring(0, 2);
        return invalidInputShortName;
    }

    public UpdateCategoryInput GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = GetValidInput();
        var tooLongNameCategory = Faker.Commerce.ProductName();
        while (tooLongNameCategory.Length <= 255)
            tooLongNameCategory = $"{tooLongNameCategory} {Faker.Commerce.ProductName()}";

        invalidInputTooLongName.Name =
            tooLongNameCategory;

        return invalidInputTooLongName;

    }

    public UpdateCategoryInput GetInvalidInputTooLongDescription()
    {
        var invalidInputTooLongDescription = GetValidInput();
        var tooLongDescriptionCategory = Faker.Commerce.ProductDescription();
        while (tooLongDescriptionCategory.Length <= 10_000)
            tooLongDescriptionCategory = $"{tooLongDescriptionCategory} {Faker.Commerce.ProductDescription()}";

        invalidInputTooLongDescription.Description =
            tooLongDescriptionCategory;

        return invalidInputTooLongDescription;

    }

}
