using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using System.Xml.Linq;
using Xunit;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;
[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;

    public CategoryTest(CategoryTestFixture categoryTestFixture) => _categoryTestFixture = categoryTestFixture;

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain","Category - Aggregates")]
    public void Instantiate()
    {
        //Arrange(Preparando os teste)
        var validCategory = _categoryTestFixture.GetValidCategory();

        var datetimeBefore = DateTime.Now;
        
        //Act(Fato o que que testar
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        //Assert (Verifica o teste)
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        category.IsActive.Should().BeTrue();
        
    }


    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        //Arrange(Preparando os teste)
        var validCategory = _categoryTestFixture.GetValidCategory();

        var datetimeBefore = DateTime.Now;

        //Act(Fato o que que testar
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        //Assert (Verifica o teste)
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        category.IsActive.Should().Be(isActive);
    }
    
    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain","Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("      ")]
    public void InstantiateErrorWhenNameIsEmpty(string?  name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        Action action = () => new DomainEntity.Category(name!, validCategory.Description);
       
        action.Should().Throw<EntityValidationException>().
            WithMessage("Name should not be empty or null");
        
    }
    
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        Action action = () => new DomainEntity.Category(validCategory.Name, null!);
        
        action.Should().Throw<EntityValidationException>().
            WithMessage("Description should not be null");
        
    }

    //Nome deve ter no mímino 3 caracteres
    [Theory(DisplayName =(nameof(InstantiateErrorWhenNameIsLessThan3Characters)))]
    [Trait("Domain","Category - Aggregates")]
    [MemberData(nameof(GetNameWithLessThan3Character),parameters:10)]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        Action action = () => new DomainEntity.Category(invalidName!, validCategory.Description);

        action.Should().Throw<EntityValidationException>().
            WithMessage("Name should be at leats 3 characters long");
        
    }
    public static IEnumerable<Object[]> GetNameWithLessThan3Character(int numberOfTests = 6)
    {
        var fixture = new CategoryTestFixture();
        
        for(int i= 0; i< numberOfTests; i++)
        {
            var isOdd = i % 2 == 1;
                yield return new object[] 
                { 
                    fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)] 
                };
        }
    }
    //Nome deve ter no máximo 255 caracteres
    [Fact(DisplayName = (nameof(InstantiateErrorWhenNameIsGreaterThan255Characters)))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256); 
        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

        action.Should().Throw<EntityValidationException>().
            WithMessage("Name should be at less or equal 255 characters long");
        
    }

    //Descrição deve ter no máximo 10.000 caracteres
    [Fact(DisplayName = (nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters)))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();

        while (invalidDescription.Length <= 10_000)
            invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";
        Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);

        action.Should().Throw<EntityValidationException>().
            WithMessage("Description should be at less or equal 10000 characters long");
        
    }

    //Habilitar isActive
    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        //Arrange(Preparando os teste)
        var validCategory = _categoryTestFixture.GetValidCategory();

        //Act(Fato o que que testar
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
        category.Activate();


        //Assert (Verifica o teste)
        category.IsActive.Should().BeTrue();
        
    }

    //Desabilitar isActive
    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        //Arrange(Preparando os teste)
        var validCategory = _categoryTestFixture.GetValidCategory();

        //Act(Fato o que que testar
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        category.Deactivate();


        //Assert (Verifica o teste)
        category.IsActive.Should().BeFalse();
        
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var categoryWithnewValues = _categoryTestFixture.GetValidCategory();

        category.Update(categoryWithnewValues.Name, categoryWithnewValues.Description);

        category.Name.Should().Be(categoryWithnewValues.Name);
        category.Description.Should().Be(categoryWithnewValues.Description);
        
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = _categoryTestFixture.GetValidCategory(); ;
        var newName = _categoryTestFixture.GetValidCategoryName();
        var currentDescription = category.Description;

        category.Update(newName);

        category.Name.Should().Be(newName);
        category.Description.Should().Be(currentDescription);
        
        

    }

    [Theory(DisplayName = nameof(UpdadeErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("      ")]
    public void UpdadeErrorWhenNameIsEmpty(string? name)
    {
        var category = _categoryTestFixture.GetValidCategory(); 
        Action action = () => category.Update(name!);

        action.Should().Throw<EntityValidationException>().
            WithMessage("Name should not be empty or null");
        
    }

    [Theory(DisplayName = (nameof(UpdateErrorWhenNameIsLessThan3Characters)))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNameWithLessThan3Character), parameters: 10)]
    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var category = _categoryTestFixture.GetValidCategory();
        Action action = () => category.Update(invalidName!, "Category Description");
        
        action.Should().Throw<EntityValidationException>().
            WithMessage("Name should be at leats 3 characters long");

    }

    [Fact(DisplayName = (nameof(UpdateErrorWhenNameIsGreaterThan255Characters)))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);
        Action action = () => category.Update(invalidName);

        action.Should().Throw<EntityValidationException>().
            WithMessage("Name should be at less or equal 255 characters long");
    }

    [Fact(DisplayName = (nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters)))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();

        while (invalidDescription.Length <= 10_000)
            invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}"; 
        Action action = () => category.Update("Category New Name", invalidDescription);

        action.Should().Throw<EntityValidationException>().
        WithMessage("Description should be at less or equal 10000 characters long");
        
    }

}

