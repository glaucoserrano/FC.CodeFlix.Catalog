using FC.Codeflix.Catalog.IntegrationTest.Application.UseCases.Category.Common;
using Xunit;

namespace FC.Codeflix.Catalog.IntegrationTest.Application.UseCases.Category.GetCategory;
[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture> { }
public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
{
    
}
