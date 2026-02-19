using MedClaim.Policy.Application.Abstractions;
using MedClaim.Policy.Application.Common;

namespace MedClaim.Policy.Application.Queries.GetPolicyById;

public sealed record GetPolicyByIdQuery(Guid PolicyId) : IQuery<PolicyDto>;