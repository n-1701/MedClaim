using MedClaim.Claims.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedClaim.Claims.Infrastructure.Persistence.Configurations;

public sealed class ClaimLineItemConfiguration : IEntityTypeConfiguration<ClaimLineItem>
{
    public void Configure(EntityTypeBuilder<ClaimLineItem> builder)
    {
        builder.ToTable("ClaimLineItems");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.ServiceCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(l => l.ProviderName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(l => l.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(l => l.ServiceDate)
            .IsRequired();
    }
}