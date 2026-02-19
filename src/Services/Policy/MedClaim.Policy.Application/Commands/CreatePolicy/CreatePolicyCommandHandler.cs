using MedClaim.Policy.Application.Abstractions;
using MedClaim.Policy.Domain.Entities;
using MedClaim.Shared.Primitives;

namespace MedClaim.Policy.Application.Commands.CreatePolicy;

public sealed class CreatePolicyCommandHandler : ICommandHandler<CreatePolicyCommand, Guid>
{
    private readonly IPolicyRepository _policyRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePolicyCommandHandler(
        IPolicyRepository policyRepository,
        IMemberRepository memberRepository,
        IUnitOfWork unitOfWork)
    {
        _policyRepository = policyRepository;
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken);
        if (member is null)
            return Result.Failure<Guid>($"Member with ID {request.MemberId} not found");

        if (request.EndDate <= request.StartDate)
            return Result.Failure<Guid>("End date must be after start date");

        var policy = PolicyE.Create(
            request.MemberId,
            request.PlanType,
            request.MaxAnnualCoverage,
            request.StartDate,
            request.EndDate);

        await _policyRepository.AddAsync(policy, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(policy.Id);
    }
}