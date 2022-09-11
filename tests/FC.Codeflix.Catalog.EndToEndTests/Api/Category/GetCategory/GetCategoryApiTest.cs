using FC.Codeflix.Catalog.Application.UseCases.Category.Commom;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.GetCategory;
[Collection(nameof(GetCategoryApiTestFixture))]
public class GetCategoryApiTest
{
    private readonly GetCategoryApiTestFixture _fixture;

    public GetCategoryApiTest(GetCategoryApiTestFixture fixture) => _fixture = fixture;

    [Fact(DisplayName =nameof(GetCategory))]
    [Trait("EndToEnd/API","Category/Get -  Endpoints")]
    public async Task GetCategory()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoryList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var exampleCategory = exampleCategoriesList[10];


        var (response, output) = await _fixture.ApiClient.Get<CategoryModelOutput>($"/categories/{exampleCategory.Id}");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(exampleCategory.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);

    }
    [Fact(DisplayName = nameof(GetCategoryThrowWhenNotFound))]
    [Trait("EndToEnd/API", "Category/Get -  Endpoints")]
    public async Task GetCategoryThrowWhenNotFound()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoryList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var ramdonGuid = Guid.NewGuid();


        var (response, output) = await _fixture.ApiClient.Get<ProblemDetails>($"/categories/{ramdonGuid}");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
        output.Title.Should().Be("Not Found");
        output.Detail.Should().Be($"Category '{ramdonGuid}' not found");
        output.Type.Should().Be("NotFound");

    }

}
