namespace FC.Codeflix.Catalog.Domain.SeedWork;

public interface IGenericRespository<TAggregate> : IRepository {
    public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);
}
