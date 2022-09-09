
using FC.Codeflix.Catalog.Infra.DataEF;
using FC.Codeflix.Catalog.Infra.DataEF.Repositories;
using ApplicationUseCases = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Xunit;
using FluentAssertions;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.SeedWork;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.IntegrationTest.Application.UseCases.Category.CreateCategory;
[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest 
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture) => _fixture = fixture;

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new ApplicationUseCases.CreateCategory(repository, unitOfWork);

        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);

        output!.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.Id.Should().NotBeEmpty();
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
    }

    [Fact(DisplayName = nameof(CreateCategoryOnlyName))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryOnlyName()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new ApplicationUseCases.CreateCategory(repository, unitOfWork);

        var input = new CreateCategoryInput(_fixture.GetValidCategoryName());

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be("");
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be("");
        dbCategory.IsActive.Should().BeTrue();
        dbCategory.Id.Should().NotBeEmpty();
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
    }
    [Fact(DisplayName = nameof(CreateCategoryOnlyNameandDescription))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryOnlyNameandDescription()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new ApplicationUseCases.CreateCategory(repository, unitOfWork);

        var input = new CreateCategoryInput(_fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);

        

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().BeTrue();
        dbCategory.Id.Should().NotBeEmpty();
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
    }
    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    [MemberData(nameof
        (CreateCategoryTestDataGenerator.GetInvalidInputs),
        parameters: 4,
        MemberType = typeof(CreateCategoryTestDataGenerator)
     )]
    public async void ThrowWhenCantInstantiateCategory(
    CreateCategoryInput input,
        string exceptionMessage
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new ApplicationUseCases.CreateCategory(repository, unitOfWork);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);

        var dbCategoriesList = _fixture.CreateDbContext(true).Categories.AsNoTracking().ToList();
        
        dbCategoriesList.Should().HaveCount(0);

    }
}
