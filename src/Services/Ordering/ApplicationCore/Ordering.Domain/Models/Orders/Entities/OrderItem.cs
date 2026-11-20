using Framework.Domain.Entities;
using Ordering.Domain.Models.Orders.Exception;
using Ordering.Domain.Models.Orders.ValueObjects;

namespace Ordering.Domain.Models.Orders.Entities
{
    public class OrderItem : Entity<OrderItemId>
    {
        private OrderItem()
        {
        }

        private OrderItem(OrderItemId id, ProductId productId, ProductVariantId productVariantId,
                    string productName, string sku, Money unitPrice, Quantity quantity) : base(id)
        {
            this.ProductId = productId;
            this.ProductVariantId = productVariantId;
            this.ProductName = productName;
            this.Sku = sku;
            this.UnitPrice = unitPrice;
            this.Quantity = quantity;
        }


        public ProductId ProductId { get; }
        public ProductVariantId ProductVariantId { get; }
        public string ProductName { get; }
        public string Sku { get; }
        public Money UnitPrice { get; }
        public Quantity Quantity { get; private set; }

        public Money TotalPrice => UnitPrice.Multiply(Quantity.Value);


        internal static OrderItem Create(ProductId productId, ProductVariantId productVariantId, string productName,
                                         string sku, Money unitPrice, Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(productId);
            ArgumentNullException.ThrowIfNull(productVariantId);
            ArgumentNullException.ThrowIfNull(unitPrice);
            ArgumentNullException.ThrowIfNull(quantity);

            if (string.IsNullOrWhiteSpace(productName))
                throw new DomainException("Product name is required.");

            if (string.IsNullOrWhiteSpace(sku))
                throw new DomainException("SKU is required.");

            return new OrderItem(
                            OrderItemId.New(),
                            productId,
                            productVariantId,
                            productName.Trim(),
                            sku.Trim().ToUpperInvariant(),
                            unitPrice,
                            quantity);
        }

        /// <summary>
        /// افزایش تعداد آیتم به اندازه مقدار مشخص‌شده
        /// </summary>
        internal void IncreaseQuantity(Quantity additionalQuantity)
        {
            ArgumentNullException.ThrowIfNull(additionalQuantity);
            Quantity = Quantity.Add(additionalQuantity);
        }
    }
}
