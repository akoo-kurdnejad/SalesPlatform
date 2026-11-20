using Ordering.Domain.Models.Orders.Creation;
using Ordering.Domain.Models.Orders.Entities;
using Ordering.Domain.Models.Orders.Enums;

namespace Ordering.Domain.Models.Orders.States
{
    internal sealed class PendingOrderState : OrderState
    {
        public override OrderStatus Status
            => OrderStatus.Pending;

        public override void AddItem(Order order, OrderItemInput itemInput)
            => order.AddItem(itemInput);

        public override void Confirm(Order order)
            => order.SetState(new ConfirmedOrderState());

        public override void Cancel(Order order, string? reason = null)
            => order.SetState(new CancelledOrderState());

        public override void Reject(Order order, string? reason = null)
            => order.SetState(new RejectedOrderState());
    }
}
