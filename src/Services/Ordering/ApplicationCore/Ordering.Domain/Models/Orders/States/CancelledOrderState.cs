using Ordering.Domain.Models.Orders.Enums;

namespace Ordering.Domain.Models.Orders.States
{
    internal sealed class CancelledOrderState : OrderState
    {
        public override OrderStatus Status => OrderStatus.Cancelled;
    }
}
