using MedClaim.Claims.Domain.Enums;
using MedClaim.Claims.Domain.Primitives;

namespace MedClaim.Claims.Domain.Events;

public sealed record ClaimStatusChangedEvent(
    Guid ClaimId,
    ClaimStatus OldStatus,
    ClaimStatus NewStatus,
    DateTime ChangedAt) : IDomainEvent;