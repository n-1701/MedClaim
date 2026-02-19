using MedClaim.Policy.Domain.Primitives;

namespace MedClaim.Policy.Domain.Entities;

public sealed class Beneficiary : Entity
{
    private Beneficiary() { }

    private Beneficiary(
        Guid id,
        Guid policyId,
        string name,
        string relationship) : base(id)
    {
        PolicyId = policyId;
        Name = name;
        Relationship = relationship;
    }

    public Guid PolicyId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Relationship { get; private set; } = string.Empty;

    public static Beneficiary Create(Guid policyId, string name, string relationship)
    {
        return new Beneficiary(Guid.NewGuid(), policyId, name, relationship);
    }
}