namespace Ordering.Domain.Models.Orders.Enums
{
    public enum OrderStatus
    {
        Pending = 1,
        AwaitingPayment = 2,
        Confirmed = 3,
        Rejected = 4,
        Cancelled = 5,
        Shipped = 6,
        Completed = 7
    }
}
