using Framework.Domain.Events;
using Ordering.Domain.Models.Orders.ValueObjects;

namespace Ordering.Domain.Models.Orders.DomainEvents
{
    public sealed record OrderPlacedDomainEvent(
                                OrderId OrderId,
                                CustomerId CustomerId,
                                Money TotalAmount) : DomainEvent;
}
