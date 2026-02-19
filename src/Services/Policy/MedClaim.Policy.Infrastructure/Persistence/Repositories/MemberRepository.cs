using MedClaim.Policy.Application.Abstractions;
using MedClaim.Policy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedClaim.Policy.Infrastructure.Persistence.Repositories;

public sealed class MemberRepository : IMemberRepository
{
    private readonly PolicyDbContext _context;

    public MemberRepository(PolicyDbContext context)
    {
        _context = context;
    }

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Member?> GetByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => m.NationalId == nationalId, cancellationToken);
    }

    public async Task AddAsync(Member member, CancellationToken cancellationToken = default)
    {
        await _context.Members.AddAsync(member, cancellationToken);
    }
}