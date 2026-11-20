using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models.Orders.Entities;
using Ordering.Domain.Models.Orders.Repository;
using Ordering.Domain.Models.Orders.ValueObjects;

namespace Ordering.DataAccess.Ef.Repositories;

public sealed class OrderRepository(OrderingDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        var order = await context.Orders
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        order?.RestoreState(order.Status);
        return order;
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await context.Orders.AddAsync(order, cancellationToken);
    }
}