using MedClaim.Policy.Domain.Entities;

namespace MedClaim.Policy.Application.Abstractions;

public interface IPolicyRepository
{
    Task<PolicyE?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PolicyE>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task AddAsync(PolicyE policy, CancellationToken cancellationToken = default);
    Task UpdateAsync(PolicyE policy, CancellationToken cancellationToken = default);
}