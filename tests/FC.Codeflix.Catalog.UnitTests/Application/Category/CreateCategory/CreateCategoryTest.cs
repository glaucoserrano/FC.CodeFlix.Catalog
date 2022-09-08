using Moq;
using Xunit;
using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FluentAssertions;
using Entity = FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Exceptions;


namespace FC.Codeflix.Catalog.UnitTests.Application.Category.CreateCategory;
[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var uniOfWorkMock = _fixture.GetUniOfWorkMock();

        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object,
            uniOfWorkMock.Object);

        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<Entity.Category>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        uniOfWorkMock.Verify(
            uow => uow.Commit(
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );


        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof
        (CreateCategoryTestDataGenerator.GetInvalidInputs),
        parameters: 24,
        MemberType = typeof(CreateCategoryTestDataGenerator)
     )]
    public async void ThrowWhenCantInstantiateCategory(
        CreateCategoryInput input,
        string exceptionMessage
    )
    {

        var useCase = new UseCases.CreateCategory(
             _fixture.GetRepositoryMock().Object,
             _fixture.GetUniOfWorkMock().Object);


        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);

    }



    [Fact(DisplayName = nameof(CreateCategoryOnlyName))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryOnlyName()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var uniOfWorkMock = _fixture.GetUniOfWorkMock();

        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object,
            uniOfWorkMock.Object);


        var input = new CreateCategoryInput(_fixture.GetValidCategoryName());

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<Entity.Category>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        uniOfWorkMock.Verify(
            uow => uow.Commit(
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );


        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be("");
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateCategoryOnlyNameandDescription))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryOnlyNameandDescription()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var uniOfWorkMock = _fixture.GetUniOfWorkMock();

        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object,
            uniOfWorkMock.Object);


        var input = new CreateCategoryInput(_fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<Entity.Category>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        uniOfWorkMock.Verify(
            uow => uow.Commit(
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );


        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }
}

