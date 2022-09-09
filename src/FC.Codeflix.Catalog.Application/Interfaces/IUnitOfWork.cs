namespace FC.Codeflix.Catalog.Application.Interfaces;
public interface IUnitOfWork
{
    public Task Commit(CancellationToken cancellationToken);

    public Task RollBack(CancellationToken cancellationToken);
}
