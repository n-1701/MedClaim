using MedClaim.Policy.Application.Abstractions;
using MedClaim.Policy.Application.Common;
using MedClaim.Shared.Primitives;

namespace MedClaim.Policy.Application.Queries.GetPolicyById;

public sealed class GetPolicyByIdQueryHandler : IQueryHandler<GetPolicyByIdQuery, PolicyDto>
{
    private readonly IPolicyRepository _policyRepository;

    public GetPolicyByIdQueryHandler(IPolicyRepository policyRepository)
    {
        _policyRepository = policyRepository;
    }

    public async Task<Result<PolicyDto>> Handle(GetPolicyByIdQuery request, CancellationToken cancellationToken)
    {
        var policy = await _policyRepository.GetByIdAsync(request.PolicyId, cancellationToken);

        if (policy is null)
            return Result.Failure<PolicyDto>($"Policy with ID {request.PolicyId} not found");

        var policyDto = new PolicyDto(
            policy.Id,
            policy.MemberId,
            policy.PlanType,
            policy.MaxAnnualCoverage,
            policy.StartDate,
            policy.EndDate,
            policy.Status,
            policy.IsActive(),
            policy.Beneficiaries.Select(b => new BeneficiaryDto(
                b.Id,
                b.Name,
                b.Relationship)));

        return Result.Success(policyDto);
    }
}