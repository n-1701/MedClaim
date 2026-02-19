using MedClaim.Policy.Domain.Entities;
using MedClaim.Policy.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace MedClaim.Policy.Infrastructure.Persistence;

public sealed class PolicyDbContext : DbContext
{
    public PolicyDbContext(DbContextOptions<PolicyDbContext> options) : base(options)
    {
    }

    public DbSet<PolicyE> Policies => Set<PolicyE>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Beneficiary> Beneficiaries => Set<Beneficiary>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PolicyDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ClearDomainEvents();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ClearDomainEvents()
    {
        var entitiesWithEvents = ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.GetDomainEvents().Any())
            .ToList();

        foreach (var entry in entitiesWithEvents)
            entry.Entity.ClearDomainEvents();
    }
}