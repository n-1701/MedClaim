namespace MedClaim.Shared.Contracts;

public sealed record ClaimStatusChangedIntegrationEvent(
    Guid ClaimId,
    string OldStatus,
    string NewStatus,
    DateTime ChangedAt);