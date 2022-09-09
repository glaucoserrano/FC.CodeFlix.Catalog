using Xunit;
using FluentAssertions;
using FC.Codeflix.Catalog.Infra.DataEF;
using Repository = FC.Codeflix.Catalog.Infra.DataEF.Repositories;
using FC.Codeflix.Catalog.Application.Exceptions;
using FCCodeflixCatalogDomainSeedWorkSearchableRepository;
using FC.Codeflix.Catalog.Domain.Entity;

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

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);

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
        var categoryRespository = new Repository.CategoryRepository(_fixture.CreateDbContext(true));

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
        var categoryRespository = new Repository.CategoryRepository(_fixture.CreateDbContext(true));

        var  task = async () => await categoryRespository.Get(exampleId, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{exampleId}' not found");
    }
    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Respositories")]
    public async Task Update()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetExampleCategory();
        var newCategoryValues = _fixture.GetExampleCategory();
        var exampleCategoryList = _fixture.GetExampleCategoriesList(15);
        exampleCategoryList.Add(exampleCategory);
        await dbContext.AddRangeAsync(exampleCategoryList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryRespository = new Repository.CategoryRepository(dbContext);

        exampleCategory.Update(newCategoryValues.Name, newCategoryValues.Description);
        await categoryRespository.Update(exampleCategory, CancellationToken.None);
        dbContext.SaveChanges();

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory!.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);

    }
    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Respositories")]
    public async Task Delete()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetExampleCategory();
        var newCategoryValues = _fixture.GetExampleCategory();
        var exampleCategoryList = _fixture.GetExampleCategoriesList(15);
        exampleCategoryList.Add(exampleCategory);
        await dbContext.AddRangeAsync(exampleCategoryList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryRespository = new Repository.CategoryRepository(dbContext);

        
        await categoryRespository.Delete(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().BeNull();
    }
    [Fact(DisplayName = nameof(SearchReturnsListAndTotal))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Respositories")]
    public async Task SearchReturnsListAndTotal()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        
        var exampleCategoryList = _fixture.GetExampleCategoriesList(15);
        await dbContext.AddRangeAsync(exampleCategoryList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryRespository = new Repository.CategoryRepository(dbContext);
        var SearchInput = new SearchInput(1, 20, "", "", Domain.SeedWork.SearchableRepository.SearchOrder.Asc);

        var output = await categoryRespository.Search(SearchInput, CancellationToken.None);
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(SearchInput.Page);
        output.PerPage.Should().Be(SearchInput.PerPage);
        output.Total.Should().Be(exampleCategoryList.Count);
        output.Items.Should().HaveCount(exampleCategoryList.Count);
        foreach(Category outputItem in output.Items)
        {
            var exampleItem = exampleCategoryList.Find(category => category.Id == outputItem.Id);

            outputItem.Should().NotBeNull();
            outputItem!.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }

    }
    [Fact(DisplayName = nameof(SearchReturnsEmptyWhenPersistenceisEmpty))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Respositories")]
    public async Task SearchReturnsEmptyWhenPersistenceisEmpty()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var categoryRespository = new Repository.CategoryRepository(dbContext);
        var SearchInput = new SearchInput(1, 20, "", "", Domain.SeedWork.SearchableRepository.SearchOrder.Asc);

        var output = await categoryRespository.Search(SearchInput, CancellationToken.None);
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(SearchInput.Page);
        output.PerPage.Should().Be(SearchInput.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
        
    }

    [Theory(DisplayName = nameof(SearchReturnsPaginated))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Respositories")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearchReturnsPaginated(int quantityCategoriesToGenerate, int page, int perPage, int expectedQuantityItems)
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategoryList = _fixture.GetExampleCategoriesList(quantityCategoriesToGenerate);
        await dbContext.AddRangeAsync(exampleCategoryList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryRespository = new Repository.CategoryRepository(dbContext);
        var SearchInput = new SearchInput(page, perPage, "", "", Domain.SeedWork.SearchableRepository.SearchOrder.Asc);

        var output = await categoryRespository.Search(SearchInput, CancellationToken.None);
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(SearchInput.Page);
        output.PerPage.Should().Be(SearchInput.PerPage);
        output.Total.Should().Be(quantityCategoriesToGenerate);
        output.Items.Should().HaveCount(expectedQuantityItems);
        foreach (Category outputItem in output.Items)
        {
            var exampleItem = exampleCategoryList.Find(category => category.Id == outputItem.Id);

            outputItem.Should().NotBeNull();
            outputItem!.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);

        }
    }
    [Theory(DisplayName = nameof(SearchByText))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Respositories")]
    [InlineData("Action", 1, 5, 1,1)]
    [InlineData("Horror", 1, 5, 3,3)]
    [InlineData("Horror", 2, 5, 0,3)]
    [InlineData("Sci-fi", 1, 5, 4,4)]
    [InlineData("Sci-fi", 1, 3, 3,4)]
    [InlineData("Sci-fi", 2, 3, 1,4)]
    [InlineData("Hero", 1, 3, 0,0)]
    [InlineData("Robots", 1, 5, 2,2)]
    public async Task SearchByText(string search , int page, int perPage, int expectedQuantityItemsReturned, int expectedQuantityTotalItems )
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategoryList = _fixture.GetExampleCategoriesListWithNames(new List<string>()
        {
            "Action",
            "Horror",
            "Horror - Robots",
            "Horror - Based on Real Facts",
            "Drama",
            "Marvel",
            "Classic",
            "Comedy",
            "Sci-fi IA",
            "Sci-fi Space",
            "Sci-fi Future",
            "Sci-fi Robots"
        });
        await dbContext.AddRangeAsync(exampleCategoryList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryRespository = new Repository.CategoryRepository(dbContext);
        var SearchInput = new SearchInput(page, perPage, search, "", Domain.SeedWork.SearchableRepository.SearchOrder.Asc);

        var output = await categoryRespository.Search(SearchInput, CancellationToken.None);
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(SearchInput.Page);
        output.PerPage.Should().Be(SearchInput.PerPage);
        output.Total.Should().Be(expectedQuantityTotalItems);
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);
        foreach (Category outputItem in output.Items)
        {
            var exampleItem = exampleCategoryList.Find(category => category.Id == outputItem.Id);

            outputItem.Should().NotBeNull();
            outputItem!.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);

        }
    }
    [Theory(DisplayName = nameof(SearchOrdered))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Respositories")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    [InlineData("", "asc")]
    public async Task SearchOrdered(string orderBy, string order)
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategoryList = _fixture.GetExampleCategoriesList(10);
        await dbContext.AddRangeAsync(exampleCategoryList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryRespository = new Repository.CategoryRepository(dbContext);
        var searchOrder = order.ToLower() == "asc" ? Domain.SeedWork.SearchableRepository.SearchOrder.Asc : Domain.SeedWork.SearchableRepository.SearchOrder.Desc;
        var SearchInput = new SearchInput(1, 20, "", orderBy, searchOrder);

        var output = await categoryRespository.Search(SearchInput, CancellationToken.None);

        var expectedOrderedList = _fixture.CloneCategoriesListOrdered(exampleCategoryList, orderBy, searchOrder);
        
        
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(SearchInput.Page);
        output.PerPage.Should().Be(SearchInput.PerPage);
        output.Total.Should().Be(exampleCategoryList.Count);
        output.Items.Should().HaveCount(exampleCategoryList.Count);
        for (int indice = 0; indice < expectedOrderedList.Count; indice++)
        {
            var expectedItem = expectedOrderedList[indice];
            var outputItem = output.Items[indice];

            expectedItem.Should().NotBeNull();
            outputItem.Should().NotBeNull();
            outputItem!.Name.Should().Be(expectedItem.Name);
            outputItem.Id.Should().Be(expectedItem.Id);
            outputItem.Description.Should().Be(expectedItem.Description);
            outputItem.IsActive.Should().Be(expectedItem.IsActive);
            outputItem.CreatedAt.Should().Be(expectedItem.CreatedAt);
        }
        
        
    }
}
