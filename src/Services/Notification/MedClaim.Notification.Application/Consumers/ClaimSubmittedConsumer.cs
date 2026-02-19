using MassTransit;
using MedClaim.Shared.Contracts;
using Microsoft.Extensions.Logging;

namespace MedClaim.Notification.Application.Consumers;

public sealed class ClaimSubmittedConsumer : IConsumer<ClaimSubmittedIntegrationEvent>
{
    private readonly ILogger<ClaimSubmittedConsumer> _logger;

    public ClaimSubmittedConsumer(ILogger<ClaimSubmittedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ClaimSubmittedIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Claim submitted notification: ClaimId={ClaimId}, MemberId={MemberId}, Amount={Amount}",
            message.ClaimId,
            message.MemberId,
            message.TotalAmount);

        // TODO: Send actual email/SMS/push notification
        // For now, just logging to demonstrate the consumer works
        Console.WriteLine($"[NOTIFICATION] Claim {message.ClaimId} submitted by Member {message.MemberId}. Total: {message.TotalAmount:C}");

        return Task.CompletedTask;
    }
}