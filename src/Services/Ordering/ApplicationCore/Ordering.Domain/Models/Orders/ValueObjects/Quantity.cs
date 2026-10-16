using Framework.Domain.ValueObjects;
using Ordering.Domain.Models.Orders.Exception;

namespace Ordering.Domain.Models.Orders.ValueObjects
{
    public sealed class Quantity : ValueObject
    {
        public int Value { get; }

        private Quantity(int value)
        {
            Value = value;
        }

        public static Quantity Create(int value)
        {
            if (value <= 0)
                throw new DomainException("Quantity must be greater than zero.");

            return new Quantity(value);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
