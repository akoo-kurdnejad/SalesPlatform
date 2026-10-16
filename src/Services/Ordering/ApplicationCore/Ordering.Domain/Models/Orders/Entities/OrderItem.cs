using Framework.Domain.Entities;
using Ordering.Domain.Models.Orders.ValueObjects;

namespace Ordering.Domain.Models.Orders.Entities
{
    public class OrderItem : Entity<OrderItemId>
    {
        public ProductId ProductId { get; private set; }

        public ProductVariantId ProductVariantId { get; private set; }

        public string ProductName { get; private set; }

        public string Sku { get; private set; }

        public Money UnitPrice { get; private set; }

        public Quantity Quantity { get; private set; }

        public Money TotalPrice => UnitPrice.Multiply(Quantity.Value);
    }
}
