using MedClaim.Policy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedClaim.Policy.Infrastructure.Persistence.Configurations;

public sealed class PolicyConfiguration : IEntityTypeConfiguration<PolicyE>
{
    public void Configure(EntityTypeBuilder<PolicyE> builder)
    {
        builder.ToTable("Policies");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PlanType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.MaxAnnualCoverage)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasMany<Domain.Entities.Beneficiary>("_beneficiaries")
            .WithOne()
            .HasForeignKey(b => b.PolicyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation("_beneficiaries").HasField("_beneficiaries");

        builder.HasIndex(p => p.MemberId);
        builder.HasIndex(p => p.Status);
    }
}