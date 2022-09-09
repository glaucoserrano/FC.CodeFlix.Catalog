using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.Infra.DataEF;
using FC.Codeflix.Catalog.Infra.DataEF.Repositories;
using Xunit;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using FC.Codeflix.Catalog.Application.Exceptions;

namespace FC.Codeflix.Catalog.IntegrationTest.Application.UseCases.Category.DeleteCategory;
[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Integration/Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategory()
    {
        var dbContext = _fixture.CreateDbContext();
        var categoryExampleList = _fixture.GetExampleCategoriesList(10);
        var categoryExample = _fixture.GetExampleCategory();
        await dbContext.AddRangeAsync(categoryExampleList);
        var tracking = await dbContext.AddAsync(categoryExample);
        await dbContext.SaveChangesAsync();
        tracking.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var input = new UseCase.DeleteCategoryInput(categoryExample.Id);
        var useCase = new UseCase.DeleteCategory(
            repository,
            unitOfWork
            );


        await useCase.Handle(input, CancellationToken.None);
        var assertDbContext = _fixture.CreateDbContext(true);

        var dbCategoryDeleted = await assertDbContext.Categories.FindAsync(input.Id);
        dbCategoryDeleted.Should().BeNull();
        
        var dbCategories = await assertDbContext.Categories.ToListAsync();
        dbCategories.Should().HaveCount(categoryExampleList.Count); 
    }
    [Fact(DisplayName = nameof(DeleteCategoryThrowsWhenNotFound))]
    [Trait("Integration/Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategoryThrowsWhenNotFound()
    {
        var dbContext = _fixture.CreateDbContext();
        var categoryExampleList = _fixture.GetExampleCategoriesList(10);
        var categoryExample = _fixture.GetExampleCategory();
        await dbContext.AddRangeAsync(categoryExampleList);
        
        await dbContext.SaveChangesAsync();
        

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var input = new UseCase.DeleteCategoryInput(Guid.NewGuid());
        var useCase = new UseCase.DeleteCategory(
            repository,
            unitOfWork
            );


        var task = async () => await useCase.Handle(input, CancellationToken.None);
        
        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{input.Id}' not found");
    }
}
