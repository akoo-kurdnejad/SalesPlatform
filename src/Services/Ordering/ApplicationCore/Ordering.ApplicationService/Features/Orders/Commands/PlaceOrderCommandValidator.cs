using FluentValidation;
using Ordering.ApplicationService.Features.Orders.Commands;

namespace Ordering.Application.Orders.Commands.PlaceOrder
{
    public sealed class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
    {
        public PlaceOrderCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("شناسه مشتری الزامی است.");

            RuleFor(x => x.ShippingAddress)
                .NotNull()
                .WithMessage("آدرس ارسال الزامی است.");

            When(x => x.ShippingAddress is not null, () =>
            {
                RuleFor(x => x.ShippingAddress.Country)
                    .NotEmpty()
                    .WithMessage("کشور الزامی است.");

                RuleFor(x => x.ShippingAddress.City)
                    .NotEmpty()
                    .WithMessage("شهر الزامی است.");

                RuleFor(x => x.ShippingAddress.Street)
                    .NotEmpty()
                    .WithMessage("نشانی خیابان الزامی است.");

                RuleFor(x => x.ShippingAddress.PostalCode)
                    .NotEmpty()
                    .WithMessage("کد پستی الزامی است.");
            });

            RuleFor(x => x.Items)
                .NotNull()
                .Must(items => items.Count > 0)
                .WithMessage("سفارش باید حداقل یک قلم کالا داشته باشد.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEmpty()
                    .WithMessage("شناسه محصول الزامی است.");

                item.RuleFor(i => i.ProductVariantId)
                    .NotEmpty()
                    .WithMessage("شناسه تنوع محصول الزامی است.");

                item.RuleFor(i => i.ProductName)
                    .NotEmpty()
                    .WithMessage("نام محصول الزامی است.");

                item.RuleFor(i => i.Sku)
                    .NotEmpty()
                    .WithMessage("کد SKU الزامی است.");

                item.RuleFor(i => i.UnitPriceAmount)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("مبلغ واحد نمی‌تواند منفی باشد.");

                item.RuleFor(i => i.Currency)
                    .NotEmpty()
                    .WithMessage("واحد ارز الزامی است.")
                    .Length(3)
                    .WithMessage("کد ارز باید ۳ کاراکتر باشد.");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .WithMessage("تعداد باید بزرگ‌تر از صفر باشد.");
            });
        }
    }
}
