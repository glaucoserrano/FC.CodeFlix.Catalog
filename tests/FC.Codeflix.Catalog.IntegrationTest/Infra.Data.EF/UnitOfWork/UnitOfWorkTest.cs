using Microsoft.EntityFrameworkCore;
using Xunit;
using UnitOfWorkInfra = FC.Codeflix.Catalog.Infra.DataEF;
using FluentAssertions;

namespace FC.Codeflix.Catalog.IntegrationTest.Infra.Data.EF.UnitOfWork;
[Collection(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTest 
{
    private UnitOfWorkTestFixture _fixture;

    public UnitOfWorkTest(UnitOfWorkTestFixture unitOfWorkTestFixture) 
        => _fixture = unitOfWorkTestFixture;
    [Fact(DisplayName =nameof(Commit))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Commit()
    {
        
        var dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
        await dbContext.AddRangeAsync(exampleCategoriesList);

        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);
        await unitOfWork.Commit(CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(true);
        var savedCategories = assertDbContext.Categories.AsNoTracking().ToList();
        savedCategories.Should().HaveCount(exampleCategoriesList.Count);
    }
    [Fact(DisplayName = nameof(RollBack))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task RollBack()
    {
        var dbContext = _fixture.CreateDbContext();
        

        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);
        var task = async () => await unitOfWork.RollBack(CancellationToken.None);

        await task.Should().NotThrowAsync();
    }
}
