using MedClaim.Claims.Domain.Primitives;

namespace MedClaim.Claims.Domain.Entities;

public sealed class ClaimLineItem : Entity
{
    private ClaimLineItem() { }

    private ClaimLineItem(
        Guid id,
        Guid claimId,
        string serviceCode,
        string providerName,
        DateTime serviceDate,
        decimal amount) : base(id)
    {
        ClaimId = claimId;
        ServiceCode = serviceCode;
        ProviderName = providerName;
        ServiceDate = serviceDate;
        Amount = amount;
    }

    public Guid ClaimId { get; private set; }
    public string ServiceCode { get; private set; } = string.Empty;
    public string ProviderName { get; private set; } = string.Empty;
    public DateTime ServiceDate { get; private set; }
    public decimal Amount { get; private set; }

    public static ClaimLineItem Create(
        Guid claimId,
        string serviceCode,
        string providerName,
        DateTime serviceDate,
        decimal amount)
    {
        return new ClaimLineItem(
            Guid.NewGuid(),
            claimId,
            serviceCode,
            providerName,
            serviceDate,
            amount);
    }
}