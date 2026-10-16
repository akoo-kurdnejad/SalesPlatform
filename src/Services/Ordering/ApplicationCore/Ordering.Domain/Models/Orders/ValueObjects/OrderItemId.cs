using Framework.Domain.ValueObjects;

namespace Ordering.Domain.Models.Orders.ValueObjects
{
    public class OrderItemId : Id<Guid>
    {
        public OrderItemId(Guid value) : base(value)
        {
        }

        public static OrderItemId New()
        {
            return new OrderItemId(Guid.NewGuid());
        }
    }
}
