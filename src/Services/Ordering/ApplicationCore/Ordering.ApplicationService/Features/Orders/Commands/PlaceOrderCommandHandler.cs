using Framework.Application.Commands;
using Ordering.ApplicationService.Common;
using Ordering.ApplicationService.DTOs.Orders;
using Ordering.Domain.Models.Orders.Creation;
using Ordering.Domain.Models.Orders.Entities;
using Ordering.Domain.Models.Orders.Repository;
using Ordering.Domain.Models.Orders.ValueObjects;

namespace Ordering.ApplicationService.Features.Orders.Commands
{
    public sealed class PlaceOrderCommandHandler(IOrderRepository orderRepository,
                                                 IUnitOfWork unitOfWork,
                                                 TimeProvider timeProvider)
        : ICommandHandler<PlaceOrderCommand, Guid>
    {
        public async Task<Guid> HandleAsync(PlaceOrderCommand command, CancellationToken cancellationToken = default)
        {
            var customerId = new CustomerId(command.CustomerId);
            var shippingAddress = MapToAddress(command.ShippingAddress);

            Address? billingAddress = command.BillingAddress is not null
                            ? MapToAddress(command.BillingAddress)
                            : null;

            var orderItemInputs = command.Items
                    .Select(item => new OrderItemInput(
                                            new ProductId(item.ProductId),
                                            new ProductVariantId(item.ProductVariantId),
                                            item.ProductName,
                                            item.Sku,
                                            Money.Create(item.UnitPriceAmount, item.Currency),
                                            Quantity.Create(item.Quantity))
                    );

            var order = Order.Create(customerId, shippingAddress, billingAddress, orderItemInputs, timeProvider.GetUtcNow());
            await orderRepository.AddAsync(order, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return order.Id.Value;
        }

        private static Address MapToAddress(PlaceOrderAddressDTO dto)
        {
            return Address.Create(
                country: dto.Country,
                province: dto.Province,
                city: dto.City,
                street: dto.Street,
                postalCode: dto.PostalCode);
        }
    }
}
