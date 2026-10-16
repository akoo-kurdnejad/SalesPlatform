using Framework.Domain.ValueObjects;
using Ordering.Domain.Models.Orders.Exception;

namespace Ordering.Domain.Models.Orders.ValueObjects
{
    public sealed class Address : ValueObject
    {
        public string Country { get; private set; } = default!;
        public string Province { get; private set; } = default!;
        public string City { get; private set; } = default!;
        public string Street { get; private set; } = default!;
        public string PostalCode { get; private set; } = default!;

        private Address()
        {
        }

        private Address(string country, string province, string city, string street, string postalCode)
        {
            Country = country;
            Province = province;
            City = city;
            Street = street;
            PostalCode = postalCode;
        }

        public static Address Create(string country, string province, string city, string street, string postalCode)
        {
            if (string.IsNullOrWhiteSpace(country))
                throw new DomainException("Country is required.");

            if (string.IsNullOrWhiteSpace(city))
                throw new DomainException("City is required.");

            if (string.IsNullOrWhiteSpace(street))
                throw new DomainException("Street is required.");

            if (string.IsNullOrWhiteSpace(postalCode))
                throw new DomainException("Postal code is required.");

            return new Address(
                country.Trim(),
                province?.Trim() ?? string.Empty,
                city.Trim(),
                street.Trim(),
                postalCode.Trim());
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Country;
            yield return Province;
            yield return City;
            yield return Street;
            yield return PostalCode;
        }
    }
}
