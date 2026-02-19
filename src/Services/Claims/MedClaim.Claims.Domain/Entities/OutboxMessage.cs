namespace MedClaim.Claims.Domain.Entities;

public sealed class OutboxMessage
{
    private OutboxMessage() { }

    private OutboxMessage(Guid id, string eventType, string payload)
    {
        Id = id;
        EventType = eventType;
        Payload = payload;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string EventType { get; private set; } = string.Empty;
    public string Payload { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public int RetryCount { get; private set; }

    public static OutboxMessage Create(string eventType, string payload)
    {
        return new OutboxMessage(Guid.NewGuid(), eventType, payload);
    }

    public void MarkAsProcessed()
    {
        ProcessedAt = DateTime.UtcNow;
    }

    public void IncrementRetry()
    {
        RetryCount++;
    }
}