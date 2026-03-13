using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))] // Define uma coleção de testes chamada "CreateCategoryTestFixture". Isso é usado para agrupar testes que compartilham o mesmo contexto ou dependências, permitindo que eles sejam executados juntos.
public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> {
}

public class CreateCategoryTestFixture : BaseFixture {
    public CreateCategoryTestFixture() : base() { }

    public string GetValidCategoryName() {
        var categoryName = Faker.Commerce.Categories(1)[0];
        while (categoryName.Length < 3) {
            categoryName = Faker.Commerce.Categories(1)[0];
        }
        if (categoryName.Length > 255) {
            categoryName = categoryName.Substring(0, 255);
        }

        return categoryName;
    }

    public string GetValidCategoryDescription() {
        var categoryDescription = Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10_000) {
            categoryDescription = categoryDescription.Substring(0, 10_000);
        }

        return categoryDescription;
    }

    public bool GetRandomBoolean() {
        return Random.Shared.Next(2) == 0; // 0 ou 1 com 50% de chance para cada valor
    }

    public CreateCategoryInput GetInput() {
        return new CreateCategoryInput(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    }

    public Mock<ICategoryRepository> GetRepositoryMock() {
        return new Mock<ICategoryRepository>();
    }

    public Mock<IUnitOfWork> GetUnitOfWorkMock() {
        return new Mock<IUnitOfWork>();
    }
}
