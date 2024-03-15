using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApp.Entities;

namespace OrderApp.Data.EntityTypeConfigurations
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Tbl_Order");
            builder
                .HasKey(e => e.Id)
                .HasName("Tbl_OrderId_PK");
            builder.Property(e => e.Id).HasColumnName("Id");
            builder.Property(e => e.ProductId).HasColumnName("ProductId");
            builder.Property(e => e.Amount).HasColumnName("Amount");
            builder.Property(e => e.Status).HasColumnName("Status");
            builder.Property(e => e.CreatedAt).HasColumnName("CreateAt");
        }
    }
}
