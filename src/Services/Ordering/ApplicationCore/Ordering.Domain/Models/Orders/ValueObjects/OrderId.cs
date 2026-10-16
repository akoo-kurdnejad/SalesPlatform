using Framework.Domain.ValueObjects;

namespace Ordering.Domain.Models.Orders.ValueObjects
{
    public class OrderId : Id<Guid>
    {
        public OrderId(Guid value): base(value)
        {
        }

        public static OrderId New()
        {
            return new OrderId(Guid.NewGuid());
        }
    }
}
