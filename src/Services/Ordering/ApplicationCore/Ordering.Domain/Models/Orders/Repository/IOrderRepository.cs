using Ordering.Domain.Models.Orders.Entities;
using Ordering.Domain.Models.Orders.ValueObjects;

namespace Ordering.Domain.Models.Orders.Repository
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken);

        Task AddAsync(Order order, CancellationToken cancellationToken);
    }
}
