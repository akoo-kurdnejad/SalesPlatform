using Ordering.Domain.Models.Orders.Enums;

namespace Ordering.Domain.Models.Orders.States
{
    internal sealed class ShippedOrderState : OrderState
    {
        public override OrderStatus Status => OrderStatus.Shipped;
    }
}
