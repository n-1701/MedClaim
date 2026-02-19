using MedClaim.Policy.Domain.Enums;

namespace MedClaim.Policy.Application.Common;

public sealed record PolicyDto(
    Guid Id,
    Guid MemberId,
    string PlanType,
    decimal MaxAnnualCoverage,
    DateTime StartDate,
    DateTime EndDate,
    PolicyStatus Status,
    bool IsActive,
    IEnumerable<BeneficiaryDto> Beneficiaries);

public sealed record BeneficiaryDto(
    Guid Id,
    string Name,
    string Relationship);

public sealed record MemberDto(
    Guid Id,
    string FullName,
    string NationalId,
    DateTime DateOfBirth);