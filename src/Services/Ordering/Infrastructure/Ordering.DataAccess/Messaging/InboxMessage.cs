namespace Ordering.DataAccess.Messaging
{
    public sealed record InboxMessage(
            Guid MessageId,
            string EventType,
            string Payload,
            DateTimeOffset ReceivedOnUtc,
            string? CorrelationId = null,
            DateTimeOffset? ProcessedOnUtc = null,
            int RetryCount = 0,
            string? Error = null);
}
