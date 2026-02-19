namespace MedClaim.Shared.Contracts;

public sealed record ClaimSubmittedIntegrationEvent(
    Guid ClaimId,
    Guid MemberId,
    Guid PolicyId,
    decimal TotalAmount,
    DateTime SubmittedAt);