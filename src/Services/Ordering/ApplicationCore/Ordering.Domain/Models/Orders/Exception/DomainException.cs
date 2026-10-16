namespace Ordering.Domain.Models.Orders.Exception
{
    public class DomainException : Framework.Domain.Exceptions.DomainException
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}
