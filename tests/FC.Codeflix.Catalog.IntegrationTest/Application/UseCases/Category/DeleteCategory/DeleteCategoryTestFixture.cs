using FC.Codeflix.Catalog.IntegrationTest.Application.UseCases.Category.Common;
using Xunit;

namespace FC.Codeflix.Catalog.IntegrationTest.Application.UseCases.Category.DeleteCategory;
[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture> { }
public class DeleteCategoryTestFixture : CategoryUseCasesBaseFixture
{
}
