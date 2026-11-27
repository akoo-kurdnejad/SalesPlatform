using Framework.Application.Commands;
using Ordering.ApplicationService.DTOs.Orders;

namespace Ordering.ApplicationService.Features.Orders.Commands
{
    public sealed record PlaceOrderCommand(
                Guid CustomerId, PlaceOrderAddressDTO ShippingAddress,
                PlaceOrderAddressDTO? BillingAddress,
                IReadOnlyList<PlaceOrderItemDTO> Items)
        : ICommand<Guid>;
}
