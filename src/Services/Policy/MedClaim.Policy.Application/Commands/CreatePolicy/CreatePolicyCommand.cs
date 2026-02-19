using MedClaim.Policy.Application.Abstractions;

namespace MedClaim.Policy.Application.Commands.CreatePolicy;

public sealed record CreatePolicyCommand(
    Guid MemberId,
    string PlanType,
    decimal MaxAnnualCoverage,
    DateTime StartDate,
    DateTime EndDate) : ICommand<Guid>;