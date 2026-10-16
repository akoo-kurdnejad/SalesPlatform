using Framework.Domain.ValueObjects;

namespace Ordering.Domain.Models.Orders.ValueObjects
{
    public class CustomerId : Id<Guid>
    {
        public CustomerId(Guid value) : base(value)
        {
        }

        public static CustomerId New()
        {
            return new CustomerId(Guid.NewGuid());
        }
    }
}
