using MedClaim.Claims.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedClaim.Claims.Infrastructure.Persistence.Configurations;

public sealed class ClaimAuditLogConfiguration : IEntityTypeConfiguration<ClaimAuditLog>
{
    public void Configure(EntityTypeBuilder<ClaimAuditLog> builder)
    {
        builder.ToTable("ClaimAuditLogs");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.OldStatus)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.NewStatus)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.ChangedBy)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(a => a.ClaimId);
    }
}