using FC.Codeflix.Catalog.Application.Interfaces;

namespace FC.Codeflix.Catalog.Infra.DataEF;
public class UnitOfWork : IUnitOfWork
{
    private readonly CodeflixCatalogDbContext _context;

    public UnitOfWork(CodeflixCatalogDbContext context) => _context = context;

    public Task Commit(CancellationToken cancellationToken) => _context.SaveChangesAsync(cancellationToken);

    public Task RollBack(CancellationToken cancellationToken) => Task.CompletedTask;
}
