erDiagram
    CLAIM ||--o{ CLAIM_LINE_ITEM : contains
    CLAIM ||--o{ CLAIM_AUDIT_LOG : tracks
    CLAIM {
        Guid Id PK
        Guid MemberId FK
        Guid PolicyId FK
        ClaimStatus Status
        string Notes
        DateTime SubmittedAt
    }
    
    CLAIM_LINE_ITEM {
        Guid Id PK
        Guid ClaimId FK
        string ServiceCode
        string ProviderName
        DateTime ServiceDate
        decimal Amount
    }
    
    CLAIM_AUDIT_LOG {
        Guid Id PK
        Guid ClaimId FK
        ClaimStatus OldStatus
        ClaimStatus NewStatus
        string ChangedBy
        DateTime ChangedAt
    }
    
    OUTBOX_MESSAGE {
        Guid Id PK
        string EventType
        string Payload
        DateTime CreatedAt
        DateTime ProcessedAt
        int RetryCount
    }
    
    POLICY ||--o{ BENEFICIARY : includes
    MEMBER ||--o{ POLICY : owns
    
    MEMBER {
        Guid Id PK
        string FullName
        string NationalId UK
        DateTime DateOfBirth
    }
    
    POLICY {
        Guid Id PK
        Guid MemberId FK
        string PlanType
        decimal MaxAnnualCoverage
        DateTime StartDate
        DateTime EndDate
        PolicyStatus Status
    }
    
    BENEFICIARY {
        Guid Id PK
        Guid PolicyId FK
        string Name
        string Relationship
    }