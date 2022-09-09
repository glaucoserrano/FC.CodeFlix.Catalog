using FC.Codeflix.Catalog.IntegrationTest.Application.UseCases.Category.Common;
using Entity = FC.Codeflix.Catalog.Domain.Entity;
using Xunit;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

namespace FC.Codeflix.Catalog.IntegrationTest.Application.UseCases.Category.ListCategories;
[CollectionDefinition(nameof(ListCategoriesTestFixture))]
public  class ListCategoriesTestFixtureCollection : ICollectionFixture<ListCategoriesTestFixture> { }
public class ListCategoriesTestFixture : CategoryUseCasesBaseFixture
{
    public List<Entity.Category> GetExampleCategoriesListWithNames(List<string> names)
          => names.Select(name =>
          {
              var category = GetExampleCategory();
              category.Update(name);
              return category;
          }).ToList();
    public List<Entity.Category> CloneCategoriesListOrdered(List<Entity.Category> categoryList, string orderBy, SearchOrder order)
    {
        var listClone = new List<Entity.Category>(categoryList);
        var OrderedEnumerable = (orderBy.ToLower(), order) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name),
            ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name),
        };

        return OrderedEnumerable.ToList();
    }
}
