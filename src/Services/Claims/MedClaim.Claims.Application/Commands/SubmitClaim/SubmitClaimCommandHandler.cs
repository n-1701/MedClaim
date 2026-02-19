using MedClaim.Claims.Application.Abstractions;
using MedClaim.Claims.Domain.Entities;
using MedClaim.Claims.Domain.Events;
using MedClaim.Shared.Contracts;
using MedClaim.Shared.Primitives;
using System.Text.Json;

namespace MedClaim.Claims.Application.Commands.SubmitClaim;

public sealed class SubmitClaimCommandHandler : ICommandHandler<SubmitClaimCommand, Guid>
{
    private readonly IClaimRepository _claimRepository;
    private readonly IOutboxRepository _outboxRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitClaimCommandHandler(
        IClaimRepository claimRepository,
        IOutboxRepository outboxRepository,
        IUnitOfWork unitOfWork)
    {
        _claimRepository = claimRepository;
        _outboxRepository = outboxRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(SubmitClaimCommand request, CancellationToken cancellationToken)
    {
        if (!request.LineItems.Any())
            return Result.Failure<Guid>("A claim must have at least one line item");

        var claim = Claim.Create(request.MemberId, request.PolicyId, request.Notes);

        foreach (var lineItem in request.LineItems)
        {
            claim.AddLineItem(
                lineItem.ServiceCode,
                lineItem.ProviderName,
                lineItem.ServiceDate,
                lineItem.Amount);
        }

        await _claimRepository.AddAsync(claim, cancellationToken);

        // Convert domain event to integration event for outbox
        var domainEvents = claim.GetDomainEvents();
        foreach (var domainEvent in domainEvents)
        {
            if (domainEvent is ClaimSubmittedEvent submittedEvent)
            {
                var integrationEvent = new ClaimSubmittedIntegrationEvent(
                    submittedEvent.ClaimId,
                    submittedEvent.MemberId,
                    claim.PolicyId,
                    claim.TotalAmount,
                    submittedEvent.SubmittedAt);

                var outboxMessage = OutboxMessage.Create(
                    nameof(ClaimSubmittedIntegrationEvent),
                    JsonSerializer.Serialize(integrationEvent));

                await _outboxRepository.AddAsync(outboxMessage, cancellationToken);
            }
        }

        claim.ClearDomainEvents();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(claim.Id);
    }
}