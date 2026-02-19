sequenceDiagram
    participant Client
    participant FastEndpoint as GetClaimEndpoint
    participant MediatR
    participant Handler as GetClaimByIdQueryHandler
    participant ClaimRepo as IClaimRepository
    participant DB as SQL Server

    Client->>FastEndpoint: GET /api/claims/{claimId}
    FastEndpoint->>MediatR: Send(GetClaimByIdQuery)
    MediatR->>Handler: Handle(query)
    Handler->>ClaimRepo: GetByIdAsync(claimId)
    ClaimRepo->>DB: SELECT * FROM Claims WHERE Id = @claimId
    DB->>DB: Include LineItems, AuditLogs
    DB-->>ClaimRepo: Claim Entity (with children)
    ClaimRepo-->>Handler: Claim?
    
    alt Claim Not Found
        Handler-->>MediatR: Result.Failure("Not found")
        MediatR-->>FastEndpoint: Result<ClaimDto> (IsFailure)
        FastEndpoint-->>Client: 404 Not Found
    else Claim Found
        Handler->>Handler: Map to ClaimDto
        Handler-->>MediatR: Result.Success(claimDto)
        MediatR-->>FastEndpoint: Result<ClaimDto>
        FastEndpoint-->>Client: 200 OK {ClaimDto}
    end