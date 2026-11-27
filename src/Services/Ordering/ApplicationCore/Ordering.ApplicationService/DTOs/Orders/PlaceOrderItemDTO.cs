namespace Ordering.ApplicationService.DTOs.Orders
{
    public sealed record PlaceOrderItemDTO(
                Guid ProductId,
                Guid ProductVariantId,
                string ProductName,
                string Sku,
                decimal UnitPriceAmount,
                string Currency,
                int Quantity);
}
