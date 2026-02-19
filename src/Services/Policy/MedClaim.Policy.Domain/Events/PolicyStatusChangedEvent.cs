using MedClaim.Policy.Domain.Enums;
using MedClaim.Policy.Domain.Primitives;

namespace MedClaim.Policy.Domain.Events;

public sealed record PolicyStatusChangedEvent(
    Guid PolicyId,
    PolicyStatus OldStatus,
    PolicyStatus NewStatus,
    DateTime ChangedAt) : IDomainEvent;