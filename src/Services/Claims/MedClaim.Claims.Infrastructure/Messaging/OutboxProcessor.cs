using MassTransit;
using MedClaim.Claims.Application.Abstractions;
using MedClaim.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MedClaim.Claims.Infrastructure.Messaging;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(10);

    public OutboxProcessor(
        IServiceScopeFactory scopeFactory,
        IPublishEndpoint publishEndpoint,
        ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessOutboxMessagesAsync(stoppingToken);
            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var messages = await outboxRepository.GetUnprocessedAsync(cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                switch (message.EventType)
                {
                    case nameof(ClaimSubmittedIntegrationEvent):
                        var submittedEvent = JsonSerializer.Deserialize<ClaimSubmittedIntegrationEvent>(message.Payload);
                        if (submittedEvent is not null)
                            await _publishEndpoint.Publish(submittedEvent, cancellationToken);
                        break;

                    case nameof(ClaimStatusChangedIntegrationEvent):
                        var statusChangedEvent = JsonSerializer.Deserialize<ClaimStatusChangedIntegrationEvent>(message.Payload);
                        if (statusChangedEvent is not null)
                            await _publishEndpoint.Publish(statusChangedEvent, cancellationToken);
                        break;

                    default:
                        _logger.LogWarning("Unknown outbox event type: {EventType}", message.EventType);
                        break;
                }

                await outboxRepository.MarkAsProcessedAsync(message.Id, cancellationToken);
            }
            catch (Exception ex)
            {
                message.IncrementRetry();
                _logger.LogError(ex, "Failed to process outbox message {MessageId}. Retry count: {RetryCount}",
                    message.Id, message.RetryCount);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}