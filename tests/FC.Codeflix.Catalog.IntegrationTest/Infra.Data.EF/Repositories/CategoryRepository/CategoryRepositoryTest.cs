using Xunit;
using FluentAssertions;
using FC.Codeflix.Catalog.Infra.DataEF;
using Repository = FC.Codeflix.Catalog.Infra.DataEF.Repositories;
using FC.Codeflix.Catalog.Application.Exceptions;

namespace FC.Codeflix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.CategoryRepository;
[Collection(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryTestFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Respositories")]
    public async Task Insert()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetExampleCategory();
        var categoryRespository = new Repository.CategoryRepository(dbContext);

        await categoryRespository.Insert(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbCategory = await dbContext.Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);

    }
    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Respositories")]
    public async Task Get()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetExampleCategory();
        var exampleCategoryList = _fixture.GetExampleCategoriesList(10);
        exampleCategoryList.Add(exampleCategory);
        await dbContext.AddRangeAsync(exampleCategoryList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryRespository = new Repository.CategoryRepository(dbContext);

        var dbCategory = await categoryRespository.Get(exampleCategory.Id, CancellationToken.None);

        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory!.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);

    }
    [Fact(DisplayName = nameof(GetThrowIfNotFind))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Respositories")]
    public async Task GetThrowIfNotFind()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleId = Guid.NewGuid();
        await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList(10));
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryRespository = new Repository.CategoryRepository(dbContext);

        var  task = async () => await categoryRespository.Get(exampleId, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{exampleId}' not found");
    }
}
