using Ordering.Domain.Models.Orders.ValueObjects;

namespace Ordering.Domain.Models.Orders.Creation
{
    public sealed record OrderItemInput(
        ProductId ProductId,
        ProductVariantId ProductVariantId,
        string ProductName,
        string Sku,
        Money UnitPrice,
        Quantity Quantity);
}
