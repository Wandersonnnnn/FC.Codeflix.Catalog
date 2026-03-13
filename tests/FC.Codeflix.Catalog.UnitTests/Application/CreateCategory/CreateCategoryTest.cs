using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FluentAssertions;
using Moq;
using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest( CreateCategoryTestFixture fixture) {

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory() {

        var respositoryMock = fixture.GetRepositoryMock();
        var unitOfWorkMock = fixture.GetUnitOfWorkMock();

        //var useCase = new UseCases.CreateCategory("Category Name", "Description Name", true);
        var useCase = new UseCases.CreateCategory(respositoryMock.Object, unitOfWorkMock.Object); // ICategoryRepository (mockado) // IUnitOfWork (mockado)

        //var input = new UseCases.CreateCategoryInput(name: "Category Name", description: "Category Description", isActive: true);
        var input = fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        respositoryMock.Verify(repository => repository.Insert(It.IsAny<Category>(),It.IsAny<CancellationToken>()), Times.Once); // Verifica se o método "Insert" do repositório foi chamado exatamente uma vez, com qualquer instância de "Category" como argumento.
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once); // Verifica se o método "Commit" do unit of work foi chamado exatamente uma vez.

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default(DateTime));
    }
}