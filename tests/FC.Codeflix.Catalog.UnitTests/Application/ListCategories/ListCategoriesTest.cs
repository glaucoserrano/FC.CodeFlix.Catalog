using FC.Codeflix.Catalog.Domain.Entity;
using Moq;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Application.ListCategories;
[Collection(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTest
{
    private readonly ListCategoriesTestFixture _fixture;

    public ListCategoriesTest(ListCategoriesTestFixture fixture) 
        =>_fixture = fixture;

    [Fact(DisplayName =nameof(List))]
    [Trait("Application", "ListCategories - Use Cases")]
    public async Task List()
    {
        var categoriesExampleList = _fixture.GetExampleCategoriesList();
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = new ListCategoriesInput(
                page:2,
                perPage: 15,
                search:"searc-example",
                sort: "name",
                dir: SearchOrder.Asc
            );
        var outputRepositorySearch = new OutputSearch<Category>(
                currentpage: input.page,
                perPage: input.perPage,
                Items: (IReadOnlyList<Category>)categoriesExampleList,
                total: 70
            );

        repositoryMock.Setup(
            x => x.Search(
                    It.Is<SearchInput>(
                        searchInput.page == input.page && 
                        searchInput.perPage == input.perPage && 
                        searchInput.search == input.search && 
                        searchInput.orderBy = input.sort && 
                        searchInput.order == input.dir
                    ),
                    It.IsAny<CancellationToken>()
                )
            ).ReturnsAsync(outputRepositorySearch);

        var useCase = new ListCategories(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.perPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
        output.Items.Foreach(outputItem =>
        {
            var repositoryCategory = outputRepositorySearch.items.Find(
                x => x.id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryCategory.Name);
            outputItem.Description.Should().Be(repositoryCategory.Description);
            outputItem.IsActive.Should().Be(repositoryCategory.IsActive);
            outputItem.CreatedAt.Should().Be(repositoryCategory.CreatedAt);
        });

        repositoryMock.Verify( x => x.Search(
            It.Is<SearchInput>(
                    searchInput.page == input.page &&
                    searchInput.perPage == input.perPage &&
                    searchInput.search == input.search &&
                    searchInput.orderBy = input.sort &&
                    searchInput.order == input.dir
                ),
            It.IsAny<CancellationToken>()
                ), Times.Once
            );
    }

}
