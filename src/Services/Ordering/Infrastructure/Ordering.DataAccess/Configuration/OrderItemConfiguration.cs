using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models.Orders.Entities;
using Ordering.Domain.Models.Orders.ValueObjects;

namespace Ordering.DataAccess.Ef.Configurations;

internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", "Ordering");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
               .HasConversion(id => id.Value, value => new OrderItemId(value))
               .ValueGeneratedNever();

        builder.Property<OrderId>("OrderId").IsRequired();

        builder.Property(i => i.ProductId)
               .HasConversion(id => id.Value, value => new ProductId(value))
               .IsRequired();

        builder.Property(i => i.ProductVariantId)
               .HasConversion(id => id.Value, value => new ProductVariantId(value))
               .IsRequired();

        builder.Property(i => i.ProductName)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(i => i.Sku)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(i => i.Quantity)
               .HasConversion(q => q.Value, value => Quantity.Create(value))
               .IsRequired();

        builder.OwnsOne(i => i.UnitPrice, money =>
        {
            money.Property(m => m.Amount)
                 .HasColumnName("UnitPrice_Amount")
                 .HasColumnType("numeric(18,2)")
                 .IsRequired();

            money.Property(m => m.Currency)
                 .HasColumnName("UnitPrice_Currency")
                 .HasMaxLength(10)
                 .IsRequired();
        });
    }
}
