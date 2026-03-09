using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Repository;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

public class CreateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : ICreateCategory {

    public async Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken) {
        var category = new DomainEntity.Category(input.Name, input.Description, input.IsActive);

        await categoryRepository.Insert(category, cancellationToken);
        await unitOfWork.Commit(cancellationToken);

        return new CreateCategoryOutput(category.Id, category.Name, category.Description, category.IsActive, category.CreatedAt);
    }
}
