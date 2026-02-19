using MedClaim.Claims.Domain.Entities;
using MedClaim.Claims.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedClaim.Claims.Infrastructure.Persistence.Configurations;

public sealed class ClaimConfiguration : IEntityTypeConfiguration<Claim>
{
    public void Configure(EntityTypeBuilder<Claim> builder)
    {
        builder.ToTable("Claims");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.MemberId)
            .IsRequired();

        builder.Property(c => c.PolicyId)
            .IsRequired();

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Notes)
            .HasMaxLength(1000);

        builder.Property(c => c.SubmittedAt)
            .IsRequired();

        builder.HasMany<ClaimLineItem>("_lineItems")
            .WithOne()
            .HasForeignKey(l => l.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<ClaimAuditLog>("_auditLogs")
            .WithOne()
            .HasForeignKey(a => a.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation("_lineItems").HasField("_lineItems");
        builder.Navigation("_auditLogs").HasField("_auditLogs");

        builder.HasIndex(c => c.MemberId);
        builder.HasIndex(c => c.Status);
    }
}