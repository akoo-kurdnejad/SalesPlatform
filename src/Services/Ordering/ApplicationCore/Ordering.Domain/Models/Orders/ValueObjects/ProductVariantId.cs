using Framework.Domain.ValueObjects;

namespace Ordering.Domain.Models.Orders.ValueObjects
{
    public class ProductVariantId : Id<Guid>
    {
        public ProductVariantId(Guid value) : base(value)
        {
        }

        public static ProductVariantId New()
        {
            return new ProductVariantId(Guid.NewGuid());
        }
    }
}
