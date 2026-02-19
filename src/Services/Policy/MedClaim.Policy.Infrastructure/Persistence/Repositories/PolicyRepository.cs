using MedClaim.Policy.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using MedClaim.Policy.Domain.Entities;

namespace MedClaim.Policy.Infrastructure.Persistence.Repositories;

public sealed class PolicyRepository : IPolicyRepository
{
    private readonly PolicyDbContext _context;

    public PolicyRepository(PolicyDbContext context)
    {
        _context = context;
    }

    public async Task<PolicyE?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Policies
            .Include("_beneficiaries")
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<PolicyE>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.Policies
            .Include("_beneficiaries")
            .Where(p => p.MemberId == memberId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(PolicyE policy, CancellationToken cancellationToken = default)
    {
        await _context.Policies.AddAsync(policy, cancellationToken);
    }

    public Task UpdateAsync(PolicyE policy, CancellationToken cancellationToken = default)
    {
        _context.Policies.Update(policy);
        return Task.CompletedTask;
    }
}