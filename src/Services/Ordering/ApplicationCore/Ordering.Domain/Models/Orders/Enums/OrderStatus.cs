namespace Ordering.Domain.Models.Orders.Enums
{
    public enum OrderStatus
    {
        Pending = 1,
        StockReserved = 2,
        AwaitingPayment = 3,
        PaymentSuccess = 4,
        PaymentFailed = 5,
        StockFailed = 6,
        Confirmed = 7,
        Shipped = 8,
        Completed = 9,
        Cancelled = 10,
        Rejected = 11
    }
}
