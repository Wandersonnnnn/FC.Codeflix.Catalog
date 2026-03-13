using FC.Codeflix.Catalog.UnitTests.Common;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

[CollectionDefinition(nameof(CategoryTestFixture))] // Define uma coleção de testes chamada "CreateCategoryTestFixture". Isso é usado para agrupar testes que compartilham o mesmo contexto ou dependências, permitindo que eles sejam executados juntos.
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture> {
}

public class CategoryTestFixture : BaseFixture {
    public CategoryTestFixture() : base() { }

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

    public DomainEntity.Category GetValidCategory() {
        return new DomainEntity.Category(GetValidCategoryName(), GetValidCategoryDescription());
    }
}
