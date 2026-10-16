using Framework.Domain.Entities;
using Ordering.Domain.Models.Orders.Enums;
using Ordering.Domain.Models.Orders.ValueObjects;

namespace Ordering.Domain.Models.Orders.Entities
{
    public sealed class Order : AggregateRoot<OrderId>
    {
        private Order()
        {
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
        /// ایتم های سفارش
        /// </summary>
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
        private readonly List<OrderItem> _items = new();
    }
}
