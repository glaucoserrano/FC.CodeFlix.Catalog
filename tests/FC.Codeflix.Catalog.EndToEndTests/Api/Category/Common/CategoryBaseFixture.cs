using FC.Codeflix.Catalog.EndToEndTests.Base;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;
public class CategoryBaseFixture : BaseFixture
{
    public CategoryPersistence Persistence;

    public CategoryBaseFixture() : base()
    {
        Persistence = new CategoryPersistence(CreateDbContext());
    }

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

    public string GetInvalidShortName() => 
        Faker.Commerce.ProductName()[..2];
    

    public string GetInvalidTooLongName()
    {
        var tooLongNameCategory = Faker.Commerce.ProductName();
        while (tooLongNameCategory.Length <= 255)
            tooLongNameCategory = $"{tooLongNameCategory} {Faker.Commerce.ProductName()}";

        return tooLongNameCategory;

    }
    
    public string GetInvalidTooLongDescription()
    {
        var tooLongDescriptionCategory = Faker.Commerce.ProductDescription();
        while (tooLongDescriptionCategory.Length <= 10_000)
            tooLongDescriptionCategory = $"{tooLongDescriptionCategory} {Faker.Commerce.ProductDescription()}";

        return tooLongDescriptionCategory;

    }
    public DomainEntity.Category GetExampleCategory() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());

    public List<DomainEntity.Category> GetExampleCategoryList(int listLength = 15)
        => Enumerable.Range(1, listLength).Select(_ => new DomainEntity.Category(
            GetValidCategoryName(), 
            GetValidCategoryDescription(), 
            GetRandomBoolean()
        )).ToList();
}
