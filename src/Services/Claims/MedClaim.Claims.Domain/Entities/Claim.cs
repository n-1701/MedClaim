using MedClaim.Claims.Domain.Enums;
using MedClaim.Claims.Domain.Events;
using MedClaim.Claims.Domain.Primitives;

namespace MedClaim.Claims.Domain.Entities;

public sealed class Claim : Entity
{
    private readonly List<ClaimLineItem> _lineItems = new();
    private readonly List<ClaimAuditLog> _auditLogs = new();

    private Claim() { }

    private Claim(
        Guid id,
        Guid memberId,
        Guid policyId,
        string notes) : base(id)
    {
        MemberId = memberId;
        PolicyId = policyId;
        Notes = notes;
        Status = ClaimStatus.Submitted;
        SubmittedAt = DateTime.UtcNow;
    }

    public Guid MemberId { get; private set; }
    public Guid PolicyId { get; private set; }
    public ClaimStatus Status { get; private set; }
    public string Notes { get; private set; } = string.Empty;
    public DateTime SubmittedAt { get; private set; }
    public decimal TotalAmount => _lineItems.Sum(x => x.Amount);

    public IReadOnlyCollection<ClaimLineItem> LineItems => _lineItems.AsReadOnly();
    public IReadOnlyCollection<ClaimAuditLog> AuditLogs => _auditLogs.AsReadOnly();

    public static Claim Create(Guid memberId, Guid policyId, string notes)
    {
        var claim = new Claim(Guid.NewGuid(), memberId, policyId, notes);

        claim.RaiseDomainEvent(new ClaimSubmittedEvent(
            claim.Id,
            claim.MemberId,
            claim.SubmittedAt));

        return claim;
    }

    public void AddLineItem(string serviceCode, string providerName, DateTime serviceDate, decimal amount)
    {
        var lineItem = ClaimLineItem.Create(Id, serviceCode, providerName, serviceDate, amount);
        _lineItems.Add(lineItem);
    }

    public void UpdateStatus(ClaimStatus newStatus, string changedBy)
    {
        if (Status == newStatus) return;

        var oldStatus = Status;
        Status = newStatus;

        var auditLog = ClaimAuditLog.Create(Id, oldStatus, newStatus, changedBy);
        _auditLogs.Add(auditLog);

        RaiseDomainEvent(new ClaimStatusChangedEvent(Id, oldStatus, newStatus, DateTime.UtcNow));
    }
}