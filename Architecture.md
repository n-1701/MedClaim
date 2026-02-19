```mermaid
graph TB
    Client[Client - Postman/Web/Mobile]
    Gateway[Gateway - YARP]
    ClaimsAPI[Claims.API]
    PolicyAPI[Policy.API]
    NotificationWorker[Notification.Worker]
    RabbitMQ[RabbitMQ]
    ClaimsDB[(Claims DB - SQL Server)]
    PolicyDB[(Policy DB - SQL Server)]

    Client -->|HTTP Request| Gateway
    Gateway -->|Route /api/claims/*| ClaimsAPI
    Gateway -->|Route /api/policies/*| PolicyAPI
    
    ClaimsAPI -->|EF Core| ClaimsDB
    ClaimsAPI -->|Publish Events via Outbox| RabbitMQ
    
    PolicyAPI -->|EF Core| PolicyDB
    
    RabbitMQ -->|Consume Events| NotificationWorker
    
    style Gateway fill:#e1f5ff
    style ClaimsAPI fill:#fff4e1
    style PolicyAPI fill:#ffe1f5
    style NotificationWorker fill:#e1ffe1
    style RabbitMQ fill:#f0e1ff