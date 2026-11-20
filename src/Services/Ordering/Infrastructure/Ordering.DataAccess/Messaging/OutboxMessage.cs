namespace Ordering.DataAccess.Messaging
{
    public sealed record OutboxMessage(
            Guid Id,
            string EventType,
            string Payload,
            DateTimeOffset OccurredOnUtc,
            string? CorrelationId = null,
            DateTimeOffset? ProcessedOnUtc = null,
            int RetryCount = 0,
            string? Error = null);
}
