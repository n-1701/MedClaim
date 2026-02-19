using MedClaim.Claims.Application.Abstractions;
using MedClaim.Claims.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedClaim.Claims.Infrastructure.Persistence.Repositories;

public sealed class ClaimRepository : IClaimRepository
{
    private readonly ClaimsDbContext _context;

    public ClaimRepository(ClaimsDbContext context)
    {
        _context = context;
    }

    public async Task<Claim?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Claims
            .Include("_lineItems")
            .Include("_auditLogs")
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Claim>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.Claims
            .Include("_lineItems")
            .Where(c => c.MemberId == memberId)
            .OrderByDescending(c => c.SubmittedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Claim claim, CancellationToken cancellationToken = default)
    {
        await _context.Claims.AddAsync(claim, cancellationToken);
    }

    public Task UpdateAsync(Claim claim, CancellationToken cancellationToken = default)
    {
        _context.Claims.Update(claim);
        return Task.CompletedTask;
    }
}