using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.SeedWork;
using FC.Codeflix.Catalog.Infra.DataEF;
using FC.Codeflix.Catalog.IntegrationTest.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FC.Codeflix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.CategoryRepository;
[CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTestFixtureCollection : ICollectionFixture<CategoryRepositoryTestFixture> { }
public class CategoryRepositoryTestFixture : BaseFixture
{
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
    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;

    public Category GetExampleCategory()
    => new(
        GetValidCategoryName(),
        GetValidCategoryDescription(),
        GetRandomBoolean()
    );
    public CodeflixCatalogDbContext CreateDbContext()
       => new(
           new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
           .UseInMemoryDatabase("integration-test-db")
           .Options
           );

    public List<Category> GetExampleCategoriesList(int length = 10)
        => Enumerable.Range(1, length).Select(_ => GetExampleCategory()).ToList();
}


