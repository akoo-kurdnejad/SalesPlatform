using Ordering.ApplicationService.Common;

namespace Ordering.DataAccess.Base
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly OrderingDbContext _dbContext;

        public UnitOfWork(OrderingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
