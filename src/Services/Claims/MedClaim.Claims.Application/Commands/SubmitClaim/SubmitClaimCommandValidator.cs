using FluentValidation;

namespace MedClaim.Claims.Application.Commands.SubmitClaim;

public sealed class SubmitClaimCommandValidator : AbstractValidator<SubmitClaimCommand>
{
    public SubmitClaimCommandValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("MemberId is required");

        RuleFor(x => x.PolicyId)
            .NotEmpty().WithMessage("PolicyId is required");

        RuleFor(x => x.LineItems)
            .NotEmpty().WithMessage("At least one line item is required");

        RuleForEach(x => x.LineItems)
            .ChildRules(lineItem =>
            {
                lineItem.RuleFor(l => l.ServiceCode)
                    .NotEmpty().WithMessage("ServiceCode is required");

                lineItem.RuleFor(l => l.ProviderName)
                    .NotEmpty().WithMessage("ProviderName is required");

                lineItem.RuleFor(l => l.Amount)
                    .GreaterThan(0).WithMessage("Amount must be greater than zero");
            });
    }
}