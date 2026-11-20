using Ordering.Domain.Models.Orders.DomainEvents;
using Ordering.Domain.Models.Orders.Entities;
using Ordering.Domain.Models.Orders.Enums;

namespace Ordering.Domain.Models.Orders.States
{
    internal sealed class ConfirmedOrderState : OrderState
    {
        public override OrderStatus Status => OrderStatus.Confirmed;

        public override void Cancel(Order order, string? reason = null)
        {
            order.SetState(new CancelledOrderState());
        }
    }
}
