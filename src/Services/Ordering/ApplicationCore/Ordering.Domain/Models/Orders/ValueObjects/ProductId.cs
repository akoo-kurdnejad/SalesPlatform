using Framework.Domain.ValueObjects;

namespace Ordering.Domain.Models.Orders.ValueObjects
{
    public class ProductId : Id<Guid>
    {
        public ProductId(Guid value) : base(value)
        {
        }

        public static ProductId New()
        {
            return new ProductId(Guid.NewGuid());
        }
    }
}
