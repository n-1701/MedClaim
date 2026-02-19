using MedClaim.Claims.Domain.Enums;
using MedClaim.Claims.Domain.Primitives;

namespace MedClaim.Claims.Domain.Entities;

public sealed class ClaimAuditLog : Entity
{
    private ClaimAuditLog() { }

    private ClaimAuditLog(
        Guid id,
        Guid claimId,
        ClaimStatus oldStatus,
        ClaimStatus newStatus,
        string changedBy,
        DateTime changedAt) : base(id)
    {
        ClaimId = claimId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
        ChangedBy = changedBy;
        ChangedAt = changedAt;
    }

    public Guid ClaimId { get; private set; }
    public ClaimStatus OldStatus { get; private set; }
    public ClaimStatus NewStatus { get; private set; }
    public string ChangedBy { get; private set; } = string.Empty;
    public DateTime ChangedAt { get; private set; }

    public static ClaimAuditLog Create(
        Guid claimId,
        ClaimStatus oldStatus,
        ClaimStatus newStatus,
        string changedBy)
    {
        return new ClaimAuditLog(
            Guid.NewGuid(),
            claimId,
            oldStatus,
            newStatus,
            changedBy,
            DateTime.UtcNow);
    }
}