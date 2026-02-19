using MedClaim.Claims.Domain.Entities;

namespace MedClaim.Claims.Application.Abstractions;

public interface IClaimRepository
{
    Task<Claim?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Claim>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task AddAsync(Claim claim, CancellationToken cancellationToken = default);
    Task UpdateAsync(Claim claim, CancellationToken cancellationToken = default);
}