using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Entity Framework configuration for Sale entity.
/// </summary>
public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    /// <summary>
    /// Configures the Sale entity.
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.SaleNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.SaleDate)
            .IsRequired();

        builder.Property(s => s.Customer)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.Branch)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt);

        // Configure the relationship with SaleItems
        builder.HasMany(s => s.Items)
            .WithOne(i => i.Sale)
            .HasForeignKey(i => i.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Create a unique index on SaleNumber
        builder.HasIndex(s => s.SaleNumber)
            .IsUnique();
    }
}
