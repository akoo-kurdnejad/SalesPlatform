using Framework.Domain.Events;
using Ordering.Domain.Models.Orders.ValueObjects;

namespace Ordering.Domain.Models.Orders.DomainEvents
{
    public sealed record OrderCancelledDomainEvent(
                                OrderId orderId,
                                CustomerId customerId,
                                string? reason = null): DomainEvent;
}
