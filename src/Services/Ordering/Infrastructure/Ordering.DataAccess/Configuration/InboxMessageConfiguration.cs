using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.DataAccess.Messaging;

namespace Ordering.DataAccess.Ef.Configurations;

internal sealed class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable("InboxMessages", "Ordering");

        builder.HasKey(x => x.MessageId);

        builder.Property(x => x.MessageId)
            .ValueGeneratedNever();

        builder.Property(x => x.EventType)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.Payload)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(x => x.CorrelationId)
            .HasMaxLength(100);

        builder.Property(x => x.ReceivedOnUtc)
            .IsRequired();

        builder.Property(x => x.RetryCount)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.Error)
            .HasMaxLength(2000);

        builder.HasIndex(x => new { x.ProcessedOnUtc, x.ReceivedOnUtc })
            .HasDatabaseName("ix_inbox_messages_unprocessed");
    }
}