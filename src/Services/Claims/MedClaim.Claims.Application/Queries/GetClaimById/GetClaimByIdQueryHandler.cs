using MedClaim.Claims.Application.Abstractions;
using MedClaim.Claims.Application.Common;
using MedClaim.Shared.Primitives;

namespace MedClaim.Claims.Application.Queries.GetClaimById;

public sealed class GetClaimByIdQueryHandler : IQueryHandler<GetClaimByIdQuery, ClaimDto>
{
    private readonly IClaimRepository _claimRepository;

    public GetClaimByIdQueryHandler(IClaimRepository claimRepository)
    {
        _claimRepository = claimRepository;
    }

    public async Task<Result<ClaimDto>> Handle(GetClaimByIdQuery request, CancellationToken cancellationToken)
    {
        var claim = await _claimRepository.GetByIdAsync(request.ClaimId, cancellationToken);

        if (claim is null)
            return Result.Failure<ClaimDto>($"Claim with ID {request.ClaimId} not found");

        var claimDto = new ClaimDto(
            claim.Id,
            claim.MemberId,
            claim.PolicyId,
            claim.Status,
            claim.Notes,
            claim.TotalAmount,
            claim.SubmittedAt,
            claim.LineItems.Select(l => new ClaimLineItemDto(
                l.Id,
                l.ServiceCode,
                l.ProviderName,
                l.ServiceDate,
                l.Amount)));

        return Result.Success(claimDto);
    }
}