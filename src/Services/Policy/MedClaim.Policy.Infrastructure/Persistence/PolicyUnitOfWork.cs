using MedClaim.Policy.Application.Abstractions;

namespace MedClaim.Policy.Infrastructure.Persistence;

public sealed class PolicyUnitOfWork : IUnitOfWork
{
    private readonly PolicyDbContext _context;

    public PolicyUnitOfWork(PolicyDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}