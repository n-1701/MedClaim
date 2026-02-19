using MedClaim.Claims.Application.Abstractions;
using MedClaim.Claims.Application.Common;

namespace MedClaim.Claims.Application.Queries.GetClaimById;

public sealed record GetClaimByIdQuery(Guid ClaimId) : IQuery<ClaimDto>;