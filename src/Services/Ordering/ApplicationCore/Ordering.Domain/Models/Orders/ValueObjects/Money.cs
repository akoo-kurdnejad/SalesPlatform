using Framework.Domain.ValueObjects;
using Ordering.Domain.Models.Orders.Exception;

namespace Ordering.Domain.Models.Orders.ValueObjects
{
    public sealed class Money : ValueObject
    {
        public decimal Amount { get; private set; }

        public string Currency { get; private set; } = default!;

        private Money()
        {
        }

        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public static Money Create(decimal amount, string currency)
        {
            if (amount < 0)
                throw new DomainException("Money amount cannot be negative.");

            if (string.IsNullOrWhiteSpace(currency))
                throw new DomainException("Currency is required.");

            currency = currency.Trim().ToUpperInvariant();

            if (currency.Length != 3)
                throw new DomainException("Currency must be a valid 3-character currency code.");

            return new Money(amount, currency);
        }

        public static Money Zero(string currency)
        {
            return Create(0, currency);
        }

        public Money Add(Money money)
        {
            EnsureSameCurrency(money);

            return Create(Amount + money.Amount, Currency);
        }

        public Money Subtract(Money money)
        {
            EnsureSameCurrency(money);

            var result = Amount - money.Amount;

            if (result < 0)
                throw new DomainException("Money amount cannot become negative.");

            return Create(result, Currency);
        }

        public Money Multiply(int multiplier)
        {
            if (multiplier <= 0)
                throw new DomainException("Multiplier must be greater than zero.");

            return Create(Amount * multiplier, Currency);
        }

        private void EnsureSameCurrency(Money money)
        {
            ArgumentNullException.ThrowIfNull(money);

            if (!string.Equals(Currency, money.Currency, StringComparison.OrdinalIgnoreCase))
            {
                throw new DomainException(
                    $"Cannot perform operation between {Currency} and {money.Currency}.");
            }
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}
