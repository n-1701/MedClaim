using MedClaim.Claims.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedClaim.Claims.Infrastructure.Persistence.Configurations;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.EventType)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(o => o.Payload)
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.ProcessedAt);

        builder.Property(o => o.RetryCount)
            .HasDefaultValue(0);

        // index for the outbox processor query — unprocessed messages only
        builder.HasIndex(o => o.ProcessedAt);
    }
}