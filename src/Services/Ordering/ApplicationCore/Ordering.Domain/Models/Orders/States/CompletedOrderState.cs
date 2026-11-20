using Ordering.Domain.Models.Orders.Enums;

namespace Ordering.Domain.Models.Orders.States
{
    internal sealed class CompletedOrderState : OrderState
    {
        public override OrderStatus Status => OrderStatus.Completed;
    }
}
