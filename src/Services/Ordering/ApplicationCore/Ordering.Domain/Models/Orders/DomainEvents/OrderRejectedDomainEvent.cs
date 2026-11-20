using Framework.Domain.Events;
using Ordering.Domain.Models.Orders.ValueObjects;

namespace Ordering.Domain.Models.Orders.DomainEvents
{
    public sealed record OrderRejectedDomainEvent(
                                OrderId OrderId,
                                CustomerId CustomerId,
                                string? Reason = null): DomainEvent;
}
