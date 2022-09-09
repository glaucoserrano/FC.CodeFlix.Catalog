using Bogus;
using FC.Codeflix.Catalog.Infra.DataEF;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.EndToEndTests.Base;
public class BaseFixture
{
    protected Faker Faker { get; set; }
    public BaseFixture()
        => Faker = new Faker("pt_BR");

    public CodeflixCatalogDbContext CreateDbContext(bool preserveData = false)
    {
        var context = new CodeflixCatalogDbContext(
           new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
           .UseInMemoryDatabase("end2end-test-db")
           .Options
           );
        return context;
    }

}
