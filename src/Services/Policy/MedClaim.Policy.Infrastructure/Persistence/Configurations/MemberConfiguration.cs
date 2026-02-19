using MedClaim.Policy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedClaim.Policy.Infrastructure.Persistence.Configurations;

public sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.FullName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.NationalId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(m => m.DateOfBirth)
            .IsRequired();

        builder.HasIndex(m => m.NationalId)
            .IsUnique();
    }
}