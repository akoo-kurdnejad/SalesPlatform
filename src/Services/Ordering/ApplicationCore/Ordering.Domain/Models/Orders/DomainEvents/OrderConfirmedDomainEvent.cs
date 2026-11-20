using Framework.Domain.Events;
using Ordering.Domain.Models.Orders.ValueObjects;

namespace Ordering.Domain.Models.Orders.DomainEvents
{
    public sealed record OrderConfirmedDomainEvent(
                                OrderId OrderId,
                                CustomerId CustomerId): DomainEvent;
}
