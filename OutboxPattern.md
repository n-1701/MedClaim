```mermaid
flowchart TD
    Start([Command Handler Executes])
    CreateEntity[Create Domain Entity - Claim.Create]
    RaiseDomainEvent[Entity Raises Domain Event Internally]
    AddToRepo[Add Entity to Repository - in memory]
    
    ConvertEvent[Convert DomainEvent to IntegrationEvent]
    CreateOutbox[Create OutboxMessage with serialized event]
    AddOutbox[Add OutboxMessage to OutboxRepository]
    
    ClearEvents[Clear Domain Events from Entity]
    SaveChanges[Call UnitOfWork.SaveChangesAsync]
    
    Transaction{Single DB Transaction}
    SaveClaim[INSERT Claim + LineItems]
    SaveOutbox[INSERT OutboxMessage]
    Commit[Commit Transaction]
    
    ReturnSuccess[Return Result.Success to caller]
    
    BackgroundService[Outbox Processor - runs every 10s]
    Query[Query unprocessed OutboxMessages]
    
    Loop{For each message}
    Deserialize[Deserialize JSON to IntegrationEvent]
    PublishRabbit[Publish to RabbitMQ]
    MarkProcessed[Update ProcessedAt timestamp]
    SaveProcessed[SaveChanges]
    
    Start --> CreateEntity
    CreateEntity --> RaiseDomainEvent
    RaiseDomainEvent --> AddToRepo
    AddToRepo --> ConvertEvent
    ConvertEvent --> CreateOutbox
    CreateOutbox --> AddOutbox
    AddOutbox --> ClearEvents
    ClearEvents --> SaveChanges
    
    SaveChanges --> Transaction
    Transaction -->|Same Transaction| SaveClaim
    Transaction -->|Same Transaction| SaveOutbox
    SaveClaim --> Commit
    SaveOutbox --> Commit
    Commit --> ReturnSuccess
    
    BackgroundService --> Query
    Query --> Loop
    Loop -->|Yes| Deserialize
    Deserialize --> PublishRabbit
    PublishRabbit --> MarkProcessed
    MarkProcessed --> SaveProcessed
    SaveProcessed --> Loop
    Loop -->|No more| BackgroundService
    
    style Transaction fill:#ffcccc
    style Commit fill:#ccffcc
    style PublishRabbit fill:#cce5ff