using MedClaim.Policy.Domain.Enums;
using MedClaim.Policy.Domain.Events;
using MedClaim.Policy.Domain.Primitives;

namespace MedClaim.Policy.Domain.Entities;

public sealed class PolicyE : Entity
{
    private readonly List<Beneficiary> _beneficiaries = new();

    private PolicyE() { }

    private PolicyE(
        Guid id,
        Guid memberId,
        string planType,
        decimal maxAnnualCoverage,
        DateTime startDate,
        DateTime endDate) : base(id)
    {
        MemberId = memberId;
        PlanType = planType;
        MaxAnnualCoverage = maxAnnualCoverage;
        StartDate = startDate;
        EndDate = endDate;
        Status = PolicyStatus.Active;
    }

    public Guid MemberId { get; private set; }
    public string PlanType { get; private set; } = string.Empty;
    public decimal MaxAnnualCoverage { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public PolicyStatus Status { get; private set; }

    public IReadOnlyCollection<Beneficiary> Beneficiaries => _beneficiaries.AsReadOnly();

    public bool IsActive() => Status == PolicyStatus.Active
                              && StartDate <= DateTime.UtcNow
                              && EndDate >= DateTime.UtcNow;

    public static PolicyE Create(
        Guid memberId,
        string planType,
        decimal maxAnnualCoverage,
        DateTime startDate,
        DateTime endDate)
    {
        var policy = new PolicyE(
            Guid.NewGuid(),
            memberId,
            planType,
            maxAnnualCoverage,
            startDate,
            endDate);

        policy.RaiseDomainEvent(new PolicyCreatedEvent(
            policy.Id,
            policy.MemberId,
            DateTime.UtcNow));

        return policy;
    }

    public void AddBeneficiary(string name, string relationship)
    {
        var beneficiary = Beneficiary.Create(Id, name, relationship);
        _beneficiaries.Add(beneficiary);
    }

    public void UpdateStatus(PolicyStatus newStatus)
    {
        if (Status == newStatus) return;

        var oldStatus = Status;
        Status = newStatus;

        RaiseDomainEvent(new PolicyStatusChangedEvent(Id, oldStatus, newStatus, DateTime.UtcNow));
    }
}