using MedClaim.Claims.Domain.Primitives;

namespace MedClaim.Claims.Domain.Events;

public sealed record ClaimSubmittedEvent(Guid ClaimId, Guid MemberId, DateTime SubmittedAt) : IDomainEvent;