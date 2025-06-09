using FluentValidation;
using WebAPI.DTOs.Subscription;

namespace WebAPI.Validators
{
    public class SubscriptionUpdateDtoValidator : AbstractValidator<SubscriptionUpdateDto>
    {
        public SubscriptionUpdateDtoValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("CustomerId must be greater than 0");

            RuleFor(x => x.SubscriptionCost)
                .GreaterThanOrEqualTo(0).WithMessage("SubscriptionCost must be non-negative");

            RuleFor(x => x.SubscriptionInterval)
                .NotEmpty().WithMessage("SubscriptionInterval is required")
                .Must(interval => interval == "month" || interval == "year")
                .WithMessage("SubscriptionInterval must be either 'month' or 'year'");

            RuleFor(x => x.WasSubscriptionPaid)
                .NotNull().WithMessage("WasSubscriptionPaid is required");
        }
    }
}