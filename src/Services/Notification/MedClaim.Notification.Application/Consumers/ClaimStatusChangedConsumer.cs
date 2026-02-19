using MassTransit;
using MedClaim.Shared.Contracts;
using Microsoft.Extensions.Logging;

namespace MedClaim.Notification.Application.Consumers;

public sealed class ClaimStatusChangedConsumer : IConsumer<ClaimStatusChangedIntegrationEvent>
{
    private readonly ILogger<ClaimStatusChangedConsumer> _logger;

    public ClaimStatusChangedConsumer(ILogger<ClaimStatusChangedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ClaimStatusChangedIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Claim status changed: ClaimId={ClaimId}, {OldStatus} -> {NewStatus}",
            message.ClaimId,
            message.OldStatus,
            message.NewStatus);

        Console.WriteLine($"[NOTIFICATION] Claim {message.ClaimId} status changed: {message.OldStatus} → {message.NewStatus}");

        return Task.CompletedTask;
    }
}