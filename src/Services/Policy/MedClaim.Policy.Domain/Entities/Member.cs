using MedClaim.Policy.Domain.Primitives;

namespace MedClaim.Policy.Domain.Entities;

public sealed class Member : Entity
{
    private Member() { }

    private Member(
        Guid id,
        string fullName,
        string nationalId,
        DateTime dateOfBirth) : base(id)
    {
        FullName = fullName;
        NationalId = nationalId;
        DateOfBirth = dateOfBirth;
    }

    public string FullName { get; private set; } = string.Empty;
    public string NationalId { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }

    public static Member Create(string fullName, string nationalId, DateTime dateOfBirth)
    {
        return new Member(Guid.NewGuid(), fullName, nationalId, dateOfBirth);
    }
}