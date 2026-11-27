namespace Ordering.ApplicationService.DTOs.Orders
{
    public sealed record PlaceOrderAddressDTO(
                string Country,
                string? Province,
                string City,
                string Street,
                string PostalCode);
}
