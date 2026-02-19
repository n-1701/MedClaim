sequenceDiagram
    participant Client
    participant FastEndpoint as SubmitClaimEndpoint
    participant MediatR
    participant Handler as SubmitClaimCommandHandler
    participant Validator as FluentValidator
    participant ClaimEntity as Claim (Domain Entity)
    participant ClaimRepo as IClaimRepository
    participant OutboxRepo as IOutboxRepository
    participant UoW as IUnitOfWork (DbContext)
    participant DB as SQL Server
    participant OutboxProcessor as Background Worker
    participant RabbitMQ

    Client->>FastEndpoint: POST /api/claims {MemberId, PolicyId, LineItems}
    FastEndpoint->>MediatR: Send(SubmitClaimCommand)
    MediatR->>Validator: Validate(command)
    
    alt Validation Fails
        Validator-->>MediatR: ValidationException
        MediatR-->>FastEndpoint: Error Response
        FastEndpoint-->>Client: 400 Bad Request
    else Validation Succeeds
        MediatR->>Handler: Handle(command)
        Handler->>ClaimEntity: Claim.Create(memberId, policyId, notes)
        ClaimEntity->>ClaimEntity: RaiseDomainEvent(ClaimSubmittedEvent)
        Handler->>ClaimEntity: AddLineItem() x N
        Handler->>ClaimRepo: AddAsync(claim)
        
        Handler->>Handler: Convert DomainEvent to IntegrationEvent
        Handler->>OutboxRepo: AddAsync(OutboxMessage)
        Handler->>ClaimEntity: ClearDomainEvents()
        
        Handler->>UoW: SaveChangesAsync()
        Note over UoW: Single Transaction
        UoW->>DB: INSERT Claim + LineItems + OutboxMessage
        DB-->>UoW: Success
        UoW-->>Handler: ClaimId
        Handler-->>MediatR: Result.Success(claimId)
        MediatR-->>FastEndpoint: Result<Guid>
        FastEndpoint-->>Client: 201 Created {ClaimId}
        
        Note over OutboxProcessor: Runs every 10 seconds (independent)
        OutboxProcessor->>OutboxRepo: GetUnprocessedAsync()
        OutboxRepo->>DB: SELECT * FROM OutboxMessages WHERE ProcessedAt IS NULL
        DB-->>OutboxRepo: List<OutboxMessage>
        OutboxRepo-->>OutboxProcessor: Unprocessed Messages
        
        loop For Each Message
            OutboxProcessor->>OutboxProcessor: Deserialize to IntegrationEvent
            OutboxProcessor->>RabbitMQ: Publish(ClaimSubmittedIntegrationEvent)
            RabbitMQ-->>OutboxProcessor: Ack
            OutboxProcessor->>OutboxRepo: MarkAsProcessedAsync(messageId)
            OutboxProcessor->>UoW: SaveChangesAsync()
        end
    end