using MedClaim.Claims.Application.Abstractions;
using MedClaim.Claims.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedClaim.Claims.Infrastructure.Persistence.Repositories;

public sealed class OutboxRepository : IOutboxRepository
{
    private readonly ClaimsDbContext _context;

    public OutboxRepository(ClaimsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        await _context.OutboxMessages.AddAsync(message, cancellationToken);
    }

    public async Task<IEnumerable<OutboxMessage>> GetUnprocessedAsync(CancellationToken cancellationToken = default)
    {
        return await _context.OutboxMessages
            .Where(o => o.ProcessedAt == null && o.RetryCount < 5)
            .OrderBy(o => o.CreatedAt)
            .Take(20)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsProcessedAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var message = await _context.OutboxMessages
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        message?.MarkAsProcessed();
    }
}