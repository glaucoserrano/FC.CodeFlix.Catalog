﻿using FC.Codeflix.Catalog.Application.UseCases.Category.Commom;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.DeleteCategory;
[Collection(nameof(DeleteCategoryApiTestFixture))]
public class DeleteCategoryApiText
{
    private readonly DeleteCategoryApiTestFixture _fixture;

    public DeleteCategoryApiText(DeleteCategoryApiTestFixture fixture) => _fixture = fixture;

    
    [Fact(DisplayName = (nameof(DeleteCategory)))]
    [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
    public async Task DeleteCategory()
    {

        var exampleCategoriesList = _fixture.GetExampleCategoryList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var exampleCategory = exampleCategoriesList[10];


        var (response, output) = await _fixture.ApiClient.Delete<object>($"/categories/{exampleCategory.Id}");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
        output.Should().BeNull();

        var persistenceCategory = await _fixture.Persistence.GetById(exampleCategory.Id);

        persistenceCategory.Should().BeNull();
    }
    [Fact(DisplayName = (nameof(DeleteThrowWhenNotFound)))]
    [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
    public async Task DeleteThrowWhenNotFound()
    {

        var exampleCategoriesList = _fixture.GetExampleCategoryList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var randomGuid = Guid.NewGuid();


        var (response, output) = await _fixture.ApiClient.Delete<ProblemDetails>($"/categories/{randomGuid}");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
        output.Title.Should().Be("Not Found");
        output.Detail.Should().Be($"Category '{randomGuid}' not found");
        output.Type.Should().Be("NotFound");
    }
}
