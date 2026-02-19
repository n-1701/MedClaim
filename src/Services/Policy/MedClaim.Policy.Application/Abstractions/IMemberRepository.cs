using MedClaim.Policy.Domain.Entities;

namespace MedClaim.Policy.Application.Abstractions;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Member?> GetByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default);
    Task AddAsync(Member member, CancellationToken cancellationToken = default);
}