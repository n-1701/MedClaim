using FluentValidation;

namespace MedClaim.Policy.Application.Commands.CreatePolicy;

public sealed class CreatePolicyCommandValidator : AbstractValidator<CreatePolicyCommand>
{
    public CreatePolicyCommandValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("MemberId is required");

        RuleFor(x => x.PlanType)
            .NotEmpty().WithMessage("PlanType is required")
            .MaximumLength(100);

        RuleFor(x => x.MaxAnnualCoverage)
            .GreaterThan(0).WithMessage("MaxAnnualCoverage must be greater than zero");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("StartDate is required");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("EndDate is required")
            .GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate");
    }
}