using MedClaim.Claims.Domain.Enums;

namespace MedClaim.Claims.Application.Common;

public sealed record ClaimDto(
    Guid Id,
    Guid MemberId,
    Guid PolicyId,
    ClaimStatus Status,
    string Notes,
    decimal TotalAmount,
    DateTime SubmittedAt,
    IEnumerable<ClaimLineItemDto> LineItems);

public sealed record ClaimLineItemDto(
    Guid Id,
    string ServiceCode,
    string ProviderName,
    DateTime ServiceDate,
    decimal Amount);