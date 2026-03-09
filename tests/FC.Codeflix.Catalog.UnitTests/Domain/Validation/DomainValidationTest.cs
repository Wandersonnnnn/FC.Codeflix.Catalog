using Bogus;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Validation;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation;

public class DomainValidationTest {

    private Faker Faker { get; set; } = new Faker("pt_BR");

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk() {
        // Arrange
        var value = Faker.Commerce.ProductName();

        // Act
        Action action = () => DomainValidation.NotNull(value, "FieldName");

        // Assert
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull() {
        // Arrange
        string? value = null;

        // Act
        Action action = () => DomainValidation.NotNull(value, "FieldName");

        // Assert
        action.Should().Throw<EntityValidationException>().WithMessage("FieldName should not be null");
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenNull(string? value) {
        // Arrange

        // Act
        Action action = () => DomainValidation.NotNullOrEmpty(value, "FieldName");

        // Assert
        action.Should().Throw<EntityValidationException>().WithMessage("FieldName should not be empty or null");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk() {
        // Arrange
        var value = Faker.Commerce.ProductName();

        // Act
        Action action = () => DomainValidation.NotNullOrEmpty(value, "FieldName");

        // Assert
        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetNamesMinLengthThrowWhenLess), parameters: 10)] // Usa um método estático dentro da mesma classe para fornecer os dados de teste
    public void MinLengthThrowWhenLess(string value, int minLength) {
        // Arrange

        // Act
        Action action = () => DomainValidation.MinLength(value, minLength, "FieldName");

        // Assert
        action.Should().Throw<EntityValidationException>().WithMessage($"FieldName should be at least {minLength} characters long");
    }

    public static IEnumerable<object[]> GetNamesMinLengthThrowWhenLess(int numberOfTests = 5) {
        yield return new object[] { "123456", 10 }; // Teste com string fixa para garantir que o teste falha por causa do comprimento, e não por causa de outros fatores
        var faker = new Faker("pt_BR");

        for (int i = 0; i < numberOfTests; i++) {
            var minLength = faker.Random.Int(5, 20);
            var example = faker.Random.String2(minLength - 1);
            yield return new object[] { example, minLength };
        }
    }

    [Theory(DisplayName = nameof(MinLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetNamesMinLengthOk), parameters: 10)] // Usa um método estático dentro da mesma classe para fornecer os dados de teste
    public void MinLengthOk(string value, int minLength) {
        // Arrange

        // Act
        Action action = () => DomainValidation.MinLength(value, minLength, "FieldName");

        // Assert
        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetNamesMinLengthOk(int numberOfTests = 5) {
        yield return new object[] { "123456", 6 }; // Teste com string fixa para garantir que o teste passa por causa do comprimento, e não por causa de outros fatores
        var faker = new Faker("pt_BR");

        for (int i = 0; i < numberOfTests; i++) {
            var minLength = faker.Random.Int(5, 20);
            var example = faker.Random.String2(minLength + 1);
            yield return new object[] { example, minLength };
        }
    }

    [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetNamesMaxLengthThrowWhenGreater), parameters: 10)] // Usa um método estático dentro da mesma classe para fornecer os dados de teste
    public void MaxLengthThrowWhenGreater(string value, int maxLength) {
        // Arrange

        // Act
        Action action = () => DomainValidation.MaxLength(value, maxLength, "FieldName");

        // Assert
        action.Should().Throw<EntityValidationException>().WithMessage($"FieldName should be less or equal {maxLength} characters long");
    }

    public static IEnumerable<object[]> GetNamesMaxLengthThrowWhenGreater(int numberOfTests = 5) {
        yield return new object[] { "123456", 5 }; // Teste com uma string fixa que tem 6 caracteres e um maxLength de 5, o que deve causar a exceção
        var faker = new Faker("pt_BR");

        for (int i = 0; i < numberOfTests; i++) {
            var maxLength = faker.Random.Int(5, 20);
            var example = faker.Random.String2(maxLength + 1);
            yield return new object[] { example, maxLength };
        }
    }

    [Theory(DisplayName = nameof(MaxLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetNamesMaxLengthOk), parameters: 10)] // Usa um método estático dentro da mesma classe para fornecer os dados de teste
    public void MaxLengthOk(string value, int maxLength) {
        // Arrange

        // Act
        Action action = () => DomainValidation.MaxLength(value, maxLength, "FieldName");

        // Assert
        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetNamesMaxLengthOk(int numberOfTests = 5) {
        yield return new object[] { "123456", 6 }; // Teste com string fixa para garantir que o teste passa por causa do comprimento, e não por causa de outros fatores
        var faker = new Faker("pt_BR");

        for (int i = 0; i < numberOfTests; i++) {
            var maxLength = faker.Random.Int(5, 20);
            var example = faker.Random.String2(maxLength - 1);
            yield return new object[] { example, maxLength };
        }
    }
}
