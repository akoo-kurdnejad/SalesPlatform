using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models.Orders.Entities;
using Ordering.Domain.Models.Orders.ValueObjects;

namespace Ordering.DataAccess.Ef.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{

    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", "Ordering");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
               .HasColumnName("Id")
               .HasConversion(id => id.Value, value => new OrderId(value))
               .ValueGeneratedNever();

        builder.Property(o => o.CustomerId)
               .HasColumnName("CustomerId")
               .HasConversion(id => id.Value, value => new CustomerId(value))
               .IsRequired();

        // چون Status به صورت => _state.Status است، از HasConversion روی پراپرتی استفاده می‌کنیم
        // و برای خواندن از دیتابیس به صورت Setter نیاز به تنظیم دسترسی یا Shadow/Backing داریم:
        builder.Property(o => o.Status)
               .HasColumnName("Status")
               .HasConversion<string>()
               .HasMaxLength(32)
               .IsRequired();

        // Money به صورت ComplexProperty / Owned
        builder.OwnsOne(o => o.TotalAmount, money =>
        {
            money.Property(m => m.Amount)
                 .HasColumnName("TotalAmount")
                 .HasColumnType("numeric(18,2)")
                 .IsRequired();

            money.Property(m => m.Currency)
                 .HasColumnName("Currency")
                 .HasMaxLength(10)
                 .IsRequired();
        });

        builder.OwnsOne(o => o.ShippingAddress, addr =>
        {
            addr.Property(a => a.Country).HasColumnName("Shipping_Country").HasMaxLength(100).IsRequired();
            addr.Property(a => a.City).HasColumnName("Shipping_City").HasMaxLength(100).IsRequired();
            addr.Property(a => a.Street).HasColumnName("Shipping_Street").HasMaxLength(250).IsRequired();
            addr.Property(a => a.PostalCode).HasColumnName("Shipping_PostalCode").HasMaxLength(20).IsRequired();
        });

        builder.OwnsOne(o => o.BillingAddress, addr =>
        {
            addr.Property(a => a.Country).HasColumnName("Billing_Country").HasMaxLength(100).IsRequired();
            addr.Property(a => a.City).HasColumnName("Billing_City").HasMaxLength(100).IsRequired();
            addr.Property(a => a.Street).HasColumnName("Billing_Street").HasMaxLength(250).IsRequired();
            addr.Property(a => a.PostalCode).HasColumnName("Billing_PostalCode").HasMaxLength(20).IsRequired();
        });

        builder.HasMany(o => o.Items)
               .WithOne()
               .HasForeignKey("OrderId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(o => o.Items)
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .HasField("_items");

        // بازسازی _state بعد از Materialize شدن Order از دیتابیس
        builder.HasAnnotation("OrderStateRestoration", true);
    }
}