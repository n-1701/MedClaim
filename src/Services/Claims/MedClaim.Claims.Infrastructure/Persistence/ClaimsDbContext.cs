using MedClaim.Claims.Domain.Entities;
using MedClaim.Claims.Domain.Primitives;
using MedClaim.Claims.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MedClaim.Claims.Infrastructure.Persistence;

public sealed class ClaimsDbContext : DbContext
{
    public ClaimsDbContext(DbContextOptions<ClaimsDbContext> options) : base(options)
    {
    }

    public DbSet<Claim> Claims => Set<Claim>();
    public DbSet<ClaimLineItem> ClaimLineItems => Set<ClaimLineItem>();
    public DbSet<ClaimAuditLog> ClaimAuditLogs => Set<ClaimAuditLog>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClaimsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConvertDomainEventsToOutboxMessages();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ConvertDomainEventsToOutboxMessages()
    {
        var entitiesWithEvents = ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.GetDomainEvents().Any())
            .ToList();

        foreach (var entry in entitiesWithEvents)
        {
            var events = entry.Entity.GetDomainEvents();

            foreach (var domainEvent in events)
            {
                var outboxMessage = OutboxMessage.Create(
                    domainEvent.GetType().Name,
                    JsonSerializer.Serialize(domainEvent, domainEvent.GetType()));

                OutboxMessages.Add(outboxMessage);
            }

            entry.Entity.ClearDomainEvents();
        }
    }
}