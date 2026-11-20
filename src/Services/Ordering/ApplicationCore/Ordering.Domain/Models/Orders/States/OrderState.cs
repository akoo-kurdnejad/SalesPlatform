using Ordering.Domain.Models.Orders.Creation;
using Ordering.Domain.Models.Orders.Entities;
using Ordering.Domain.Models.Orders.Enums;
using Ordering.Domain.Models.Orders.Exception;

namespace Ordering.Domain.Models.Orders.States
{
    internal abstract class OrderState
    {
        public abstract OrderStatus Status { get; }

        public virtual void AddItem(Order order, OrderItemInput itemInput)
            => throw new DomainException($"Order in '{Status}' status cannot be modified.");

        public virtual void Confirm(Order order)
            => throw new DomainException($"Order in '{Status}' status cannot be confirmed.");

        public virtual void Cancel(Order order, string? reason = null)
            => throw new DomainException($"Order in '{Status}' status cannot be cancelled.");

        public virtual void Reject(Order order, string? reason = null)
            => throw new DomainException($"Order in '{Status}' status cannot be rejected.");
    }
}
