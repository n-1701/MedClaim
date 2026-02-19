```mermaid
graph LR
    Domain[Domain Layer<br/>Entities, Enums, Events]
    Application[Application Layer<br/>Commands, Queries, Interfaces]
    Infrastructure[Infrastructure Layer<br/>EF Core, Repos, MassTransit]
    API[API Layer<br/>FastEndpoints, Program.cs]
    
    Application -->|References| Domain
    Infrastructure -->|References| Application
    Infrastructure -->|Implements| Application
    API -->|References| Infrastructure
    
    style Domain fill:#e1f5e1
    style Application fill:#e1e5ff
    style Infrastructure fill:#ffe1e1
    style API fill:#fff5e1