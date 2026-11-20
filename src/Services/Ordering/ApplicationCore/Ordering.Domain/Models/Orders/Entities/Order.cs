using Framework.Domain.Entities;
using Ordering.Domain.Models.Orders.Creation;
using Ordering.Domain.Models.Orders.DomainEvents;
using Ordering.Domain.Models.Orders.Enums;
using Ordering.Domain.Models.Orders.Exception;
using Ordering.Domain.Models.Orders.States;
using Ordering.Domain.Models.Orders.ValueObjects;

namespace Ordering.Domain.Models.Orders.Entities
{
    public sealed class Order : AggregateRoot<OrderId>
    {
        private Order()
        {
        }

        private Order(OrderId id,CustomerId customerId,Address shippingAddress,Address billingAddress,
                      DateTimeOffset createdAt,IEnumerable<OrderItem> items) : base(id)
        {
            this.CustomerId = customerId;
            this.ShippingAddress = shippingAddress;
            this.BillingAddress = billingAddress;
            this.CreatedAt = createdAt;
            this.Status = OrderStatus.Pending;
            this._items.AddRange(items);
            this.TotalAmount = SumItemsTotal(_items);
        }

        /// <summary>
        /// مشتری
        /// </summary>
        public CustomerId CustomerId { get; private set; }

        /// <summary>
        /// وضعیت سفارش
        /// </summary>
        public OrderStatus Status { get; private set; }

        /// <summary>
        /// آدرس ارسال سفارش
        /// </summary>
        public Address ShippingAddress { get; private set; }

        /// <summary>
        /// آدرس ارسال صورت حساب
        /// </summary>
        public Address BillingAddress { get; private set; }

        /// <summary>
        /// تاریخ و ساعت ایجاد سفارش
        /// </summary>
        public DateTimeOffset CreatedAt { get; private set; }

        /// <summary>
        /// مبلغ کل سفارش
        /// </summary>
        public Money TotalAmount { get; private set; }

        /// <summary>
        /// ایتم های سفارش
        /// </summary>
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
        private readonly List<OrderItem> _items = new();
        private OrderState _state = new PendingOrderState();

        #region Behavior
        public static Order Create(CustomerId customerId,
                                   Address shippingAddress,
                                   Address? billingAddress,
                                   IEnumerable<OrderItemInput> items,
                                   DateTimeOffset createdAt)
        {
            ArgumentNullException.ThrowIfNull(customerId);
            ArgumentNullException.ThrowIfNull(shippingAddress);
            ArgumentNullException.ThrowIfNull(items);

            var inputList = items.ToList();
            if (inputList.Count == 0)
                throw new DomainException("Order must contain at least one item.");

            var orderItems = inputList
                    .Select(x => OrderItem.Create(
                        x.ProductId,
                        x.ProductVariantId,
                        x.ProductName,
                        x.Sku,
                        x.UnitPrice,
                        x.Quantity))
                    .ToList();

            EnsureSingleCurrency(orderItems);

            var effectiveBillingAddress = billingAddress ?? shippingAddress;

            var order = new Order(
                    OrderId.New(),
                    customerId,
                    shippingAddress,
                    effectiveBillingAddress,
                    createdAt,
                    orderItems);

            order.RaiseDomainEvent(new OrderPlacedDomainEvent(
                        order.Id,
                        order.CustomerId,
                        order.TotalAmount));

            return order;
        }

        public void AddItem(OrderItemInput itemInput)
        {
            ArgumentNullException.ThrowIfNull(itemInput);
            EnsureOrderCanBeModified();
            EnsureCurrencyMatchesExistingItems(itemInput.UnitPrice.Currency);

            var existingItem = _items
                    .FirstOrDefault(x => x.ProductId == itemInput.ProductId &&
                                         x.ProductVariantId == itemInput.ProductVariantId);

            if (existingItem is not null)
            {
                existingItem.IncreaseQuantity(itemInput.Quantity);
            }
            else
            {
                var newItem = OrderItem.Create(
                                    itemInput.ProductId,
                                    itemInput.ProductVariantId,
                                    itemInput.ProductName,
                                    itemInput.Sku,
                                    itemInput.UnitPrice,
                                    itemInput.Quantity);

                _items.Add(newItem);
            }
            this.TotalAmount = SumItemsTotal(_items);
        }

        public void Confirm()
        {
            var previousStatus = Status;
            _state.Confirm(this);
            if (Status != previousStatus && Status == OrderStatus.Confirmed)
                RaiseDomainEvent(new OrderConfirmedDomainEvent(Id, CustomerId));
        }

        public void Cancel(string? reason = null)
        {
            var previousStatus = Status;
            this._state.Cancel(this, reason);
            if (Status != previousStatus && Status == OrderStatus.Cancelled)
                RaiseDomainEvent(new OrderCancelledDomainEvent(Id, CustomerId, reason));
        }

        public void Reject(string? reason = null)
        {
            var previousStatus = Status;
            _state.Reject(this, reason);
            if (Status != previousStatus && Status == OrderStatus.Rejected)
                RaiseDomainEvent(new OrderRejectedDomainEvent(Id, CustomerId, reason));
        }

        internal void SetState(OrderState newState) => this._state = newState;

        private static Money SumItemsTotal(IReadOnlyList<OrderItem> items)
        {
            var itemList = items.ToList();

            if (items.Count == 0)
                throw new DomainException("Order must contain at least one item.");

            return itemList
                .Select(item => item.TotalPrice)
                .Aggregate((total, next) => total.Add(next));
        }

        public void RestoreState(OrderStatus status)
        {
            _state = status switch
            {
                OrderStatus.Pending => new PendingOrderState(),
                OrderStatus.Confirmed => new ConfirmedOrderState(),
                OrderStatus.Shipped => new ShippedOrderState(),
                OrderStatus.Completed => new CompletedOrderState(),
                OrderStatus.Cancelled => new CancelledOrderState(),
                OrderStatus.Rejected => new RejectedOrderState(),
                _ => throw new DomainException($"Unsupported order status '{status}'.")
            };
        }
        #endregion Behavior

        #region BusinessRules
        private static void EnsureSingleCurrency(IEnumerable<OrderItem> items)
        {
            var itemList = items.ToList();
            if (itemList.Count <= 1)
                return;

            var expectedCurrency = itemList[0].UnitPrice.Currency;

            if (itemList.Skip(1).Any(item =>
                    !string.Equals(item.UnitPrice.Currency, expectedCurrency, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DomainException("All order items must have the same currency.");
            }
        }

        private void EnsureOrderCanBeModified()
        {
            if (Status == OrderStatus.Confirmed)
                throw new DomainException("Confirmed order items cannot be modified.");

            if (Status == OrderStatus.Completed)
                throw new DomainException("Completed order cannot be modified.");

            if (Status is OrderStatus.Cancelled or OrderStatus.Rejected)
                throw new DomainException($"Order in '{Status}' status cannot be modified.");
        }

        private void EnsureCurrencyMatchesExistingItems(string currency)
        {
            if (_items.Count == 0)
                return;

            var expectedCurrency = _items[0].UnitPrice.Currency;
            if (!string.Equals(currency, expectedCurrency, StringComparison.OrdinalIgnoreCase))
                throw new DomainException("New item currency must match the existing order currency.");
        }

        #endregion BusinessRules
    }
}
