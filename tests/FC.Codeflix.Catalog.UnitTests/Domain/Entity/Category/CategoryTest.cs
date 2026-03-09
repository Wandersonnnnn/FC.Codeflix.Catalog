using Bogus;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest(CategoryTestFixture categoryTestFixture) {

    //private readonly CategoryTestFixture _categoryTestFixture;

    //public CategoryTest(CategoryTestFixture categoryTestFixture) {
        //_categoryTestFixture = categoryTestFixture;
    //}

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate() {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;

        // Act
        var category = categoryTestFixture.GetValidCategory();
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(category.Name);
        category.Description.Should().Be(category.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBe(default(DateTime));
        category.CreatedAt.Should().BeAfter(dateTimeBefore).And.BeBefore(dateTimeAfter);
        category.IsActive.Should().BeTrue();

        /*
        Assert.NotNull(category);
        Assert.Equal(category.Name, category.Name);
        Assert.Equal(category.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.True(category.IsActive);
        */
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive) {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;

        // Act
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBe(default(DateTime));
        category.CreatedAt.Should().BeAfter(dateTimeBefore).And.BeBefore(dateTimeAfter);
        category.IsActive.Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameisEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void InstantiateErrorWhenNameisEmpty(string? name) {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();

        // Act
        Action action =
            () => new DomainEntity.Category(name!, validCategory.Description);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");

        /*
        // Arrange
        var validCategory = new DomainEntity.Category("Category Name", "Category Description");
        
        // Act
        var exception = Assert.Throws<EntityValidationException>(action);

        // Assert
        Assert.Equal("Name should not be empty or null", exception.Message);
        */
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionisEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionisEmpty() {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();

        // Act
        Action action =
            () => new DomainEntity.Category(validCategory.Name, null!);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should not be null");
    }

    // nome deve ter no minimo 3 caracteres
    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]

    // Temos 3 tipos de data sources para o Theory tests no xUnit:

    /*
    [InlineData("1")]
    [InlineData("12")] // Dados fornecidos diretamente na anotação
    [InlineData("a")]
    [InlineData("ab")]*/

    //[ClassData()]  / Cria uma classe separada para fornecer os dados de teste

    [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)] // Usa um método estático dentro da mesma classe para fornecer os dados de teste
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName) {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();

        // Act
        Action action =
            () => new DomainEntity.Category(invalidName, validCategory.Description);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be at least 3 characters long");
    }

    //Gera nomes inválidos com menos de 3 caracteres - GENERATOR DE DADOS
    public static IEnumerable<object[]> GetNamesWithLessThan3Characters(int numberOfTests = 6) {
        var fixture = new CategoryTestFixture();
        for (int i = 0; i < numberOfTests; i++) {
            var indiceImpar = i % 2 == 1;
            yield return new object[] { fixture.GetValidCategoryName().Substring(0, indiceImpar ? 1 : 2) }; // Gera um nome inválido com 1 ou 2 caracteres
        }

        /*
        yield return new object[] { "1" }; // Cada linha representa um conjunto de parâmetros para o teste
        yield return new object[] { "12" };
        yield return new object[] { "a" };
        yield return new object[] { "ab" };
        yield return new object[] { "ux" };
        */
    }

    // nome deve ter no maximo 255 caracteres
    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters() {
        // Arrange
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        var validCategory = categoryTestFixture.GetValidCategory();

        // Act
        Action action =
            () => new DomainEntity.Category(invalidName, validCategory.Description);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");
    }

    // descrição deve ter no maximo 10_000 caracteres
    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters() {
        // Arrange
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        var validCategory = categoryTestFixture.GetValidCategory();

        // Act
        Action action =
            () => new DomainEntity.Category(validCategory.Name, invalidDescription);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10000 characters long");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate() {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();

        // Act
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
        category.Activate();

        // Assert
        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate() {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();

        // Act
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        category.Deactivate();

        // Assert
        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update() {
        // Arrange
        var category = categoryTestFixture.GetValidCategory();
        var categoryWithNewValues = categoryTestFixture.GetValidCategory();

        // Act
        category.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);

        // Assert
        category.Name.Should().Be(categoryWithNewValues.Name);
        category.Description.Should().Be(categoryWithNewValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName() {
        // Arrange
        var category = categoryTestFixture.GetValidCategory();
        var newName = categoryTestFixture.GetValidCategoryName();
        var currentDescription = category.Description;

        // Act
        category.Update(newName);

        // Assert
        category.Name.Should().Be(newName);
        category.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameisEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateErrorWhenNameisEmpty(string? name) {
        // Arrange
        var category = categoryTestFixture.GetValidCategory();

        // Act
        Action action =
            () => category.Update(name!);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    // nome deve ter no minimo 3 caracteres
    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ab")]
    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName) {
        // Arrange
        var category = categoryTestFixture.GetValidCategory();

        // Act
        Action action =
            () => category.Update(invalidName);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be at least 3 characters long");
    }

    // nome deve ter no maximo 255 caracteres
    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters() {
        // Arrange
        var category = categoryTestFixture.GetValidCategory();
        var invalidName = categoryTestFixture.Faker.Lorem.Letter(256);

        // Act
        Action action =
            () => category.Update(invalidName);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");
    }

    // descrição deve ter no maximo 10_000 caracteres
    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters() {
        // Arrange
        var category = categoryTestFixture.GetValidCategory();
        var invalidDescription = categoryTestFixture.Faker.Commerce.ProductDescription();
        while (invalidDescription.Length <= 10_000) {
            invalidDescription += categoryTestFixture.Faker.Commerce.ProductDescription();
        }

        // Act
        Action action =
            () => category.Update("Category Name", invalidDescription);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10000 characters long");
    }
}
