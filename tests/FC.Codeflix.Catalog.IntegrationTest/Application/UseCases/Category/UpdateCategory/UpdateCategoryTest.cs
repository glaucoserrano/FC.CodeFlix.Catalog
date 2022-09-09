using EntityCategory = FC.Codeflix.Catalog.Domain.Entity;
using Xunit;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Infra.DataEF;
using FC.Codeflix.Catalog.Infra.DataEF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.IntegrationTest.Application.UseCases.Category.UpdateCategory;
[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture) => _fixture = fixture;

    [Theory(DisplayName = nameof(UpdateCategory))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(
    nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
    parameters: 4,
    MemberType = typeof(UpdateCategoryTestDataGenerator)
)]
    public async Task UpdateCategory(
    EntityCategory.Category exampleCategory,
    UseCase.UpdateCategoryInput input
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleCategoryList = _fixture.GetExampleCategoriesList(10);
        await dbContext.AddRangeAsync(exampleCategoryList);
        var trackingInfo =  await dbContext.AddAsync(exampleCategory);
        dbContext.SaveChanges();
        trackingInfo.State = EntityState.Detached;
        var unitOfWork = new UnitOfWork(dbContext);
        var repository = new CategoryRepository(dbContext);

    
        var useCase = new UseCase.UpdateCategory(
            repository,
            unitOfWork
            );

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)input.IsActive!);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);

    }
    [Theory(DisplayName = nameof(UpdateCategoryWithoutIsActive))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(
    nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
    parameters: 4,
    MemberType = typeof(UpdateCategoryTestDataGenerator)
)]
    public async Task UpdateCategoryWithoutIsActive(
    EntityCategory.Category exampleCategory,
    UseCase.UpdateCategoryInput exampleInput
    )
    {
        var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name, exampleInput.Description);

        var dbContext = _fixture.CreateDbContext();
        var exampleCategoryList = _fixture.GetExampleCategoriesList(10);
        await dbContext.AddRangeAsync(exampleCategoryList);
        var trackingInfo = await dbContext.AddAsync(exampleCategory);
        dbContext.SaveChanges();
        trackingInfo.State = EntityState.Detached;
        var unitOfWork = new UnitOfWork(dbContext);
        var repository = new CategoryRepository(dbContext);


        var useCase = new UseCase.UpdateCategory(
            repository,
            unitOfWork
            );

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)exampleCategory.IsActive!);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);

    }
    [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(
    nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
    parameters: 4,
    MemberType = typeof(UpdateCategoryTestDataGenerator)
)]
    public async Task UpdateCategoryOnlyName(
    EntityCategory.Category exampleCategory,
    UseCase.UpdateCategoryInput exampleInput
    )
    {
        var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name);

        var dbContext = _fixture.CreateDbContext();
        var exampleCategoryList = _fixture.GetExampleCategoriesList(10);
        await dbContext.AddRangeAsync(exampleCategoryList);
        var trackingInfo = await dbContext.AddAsync(exampleCategory);
        dbContext.SaveChanges();
        trackingInfo.State = EntityState.Detached;
        var unitOfWork = new UnitOfWork(dbContext);
        var repository = new CategoryRepository(dbContext);


        var useCase = new UseCase.UpdateCategory(
            repository,
            unitOfWork
            );

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be((bool)exampleCategory.IsActive!);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);

    }
    [Fact(DisplayName = nameof(UpdateTrhowsWhenNotFoundCategory))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    public async Task UpdateTrhowsWhenNotFoundCategory()
    {
        var input = _fixture.GetValidInput();

        var dbContext = _fixture.CreateDbContext();
        var exampleCategoryList = _fixture.GetExampleCategoriesList(10);
        await dbContext.AddRangeAsync(exampleCategoryList);
        dbContext.SaveChanges();
        var unitOfWork = new UnitOfWork(dbContext);
        var repository = new CategoryRepository(dbContext);


        var useCase = new UseCase.UpdateCategory(
            repository,
            unitOfWork
            );

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{input.Id}' not found");
    }
    [Theory(DisplayName = nameof(UpdateThrowsCantInstatiateCategory))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(
    nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs),
    parameters: 6,
    MemberType = typeof(UpdateCategoryTestDataGenerator)
)]
    public async Task UpdateThrowsCantInstatiateCategory(
    UseCase.UpdateCategoryInput Input,
    string ExceptionMessage
    )
    {
        

        var dbContext = _fixture.CreateDbContext();
        var exampleCategoryList = _fixture.GetExampleCategoriesList(10);
        await dbContext.AddRangeAsync(exampleCategoryList);
        dbContext.SaveChanges();

        var unitOfWork = new UnitOfWork(dbContext);
        var repository = new CategoryRepository(dbContext);


        var useCase = new UseCase.UpdateCategory(
            repository,
            unitOfWork
            );
        Input.Id = exampleCategoryList[0].Id;
        var task = async () => await useCase.Handle(Input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(ExceptionMessage);
    }
}
