using MedClaim.Claims.Application.Abstractions;

namespace MedClaim.Claims.Infrastructure.Persistence;

public sealed class ClaimsUnitOfWork : IUnitOfWork
{
    private readonly ClaimsDbContext _context;

    public ClaimsUnitOfWork(ClaimsDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}