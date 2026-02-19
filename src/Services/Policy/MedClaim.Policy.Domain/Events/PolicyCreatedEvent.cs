using MedClaim.Policy.Domain.Primitives;

namespace MedClaim.Policy.Domain.Events;

public sealed record PolicyCreatedEvent(
    Guid PolicyId,
    Guid MemberId,
    DateTime CreatedAt) : IDomainEvent;