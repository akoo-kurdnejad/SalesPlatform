using Framework.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.DataAccess.Messaging;
using System.Text.Json;

namespace Ordering.DataAccess.Interceptors
{
    internal sealed class OutboxDomainEventInterceptor : SaveChangesInterceptor
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
                AddDomainEventsToOutbox(eventData.Context);

            return ValueTask.FromResult(result);
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context is not null)
                AddDomainEventsToOutbox(eventData.Context);
            return result;
        }

        private void AddDomainEventsToOutbox(DbContext context)
        {
            var aggregates = context.ChangeTracker
                    .Entries()
                    .Where(e => e.Entity is IAggregateRoot aggregate && aggregate.DomainEvents.Count > 0)
                    .Select(e => (IAggregateRoot)e.Entity)
                    .ToList();

            if (aggregates.Count == 0)
                return;

            var domainEvents = aggregates
                    .SelectMany(a => a.DomainEvents)
                    .ToList();

            // پاک کردن رویدادها از Aggregateها تا دوبار پردازش نشوند
            foreach (var aggregate in aggregates)
                aggregate.ClearDomainEvents();

            var now = DateTimeOffset.UtcNow;
            var outboxSet = context.Set<OutboxMessage>();

            foreach (var domainEvent in domainEvents)
            {
                var eventType = domainEvent.GetType().AssemblyQualifiedName
                        ?? domainEvent.GetType().FullName
                        ?? domainEvent.GetType().Name;

                var payload = JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), SerializerOptions);

                var outboxMessage = new OutboxMessage(
                        Id: Guid.NewGuid(),
                        EventType: eventType,
                        Payload: payload,
                        OccurredOnUtc: now);

                outboxSet.Add(outboxMessage);
            }
        }
    }
}
