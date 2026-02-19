using MedClaim.Claims.Application.Abstractions;

namespace MedClaim.Claims.Application.Commands.SubmitClaim;

public sealed record SubmitClaimCommand(
    Guid MemberId,
    Guid PolicyId,
    string Notes,
    IEnumerable<ClaimLineItemRequest> LineItems) : ICommand<Guid>;

public sealed record ClaimLineItemRequest(
    string ServiceCode,
    string ProviderName,
    DateTime ServiceDate,
    decimal Amount);