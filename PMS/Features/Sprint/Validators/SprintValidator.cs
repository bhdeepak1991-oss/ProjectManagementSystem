using FluentValidation;

namespace PMS.Features.Sprint.Validators
{
    public class SprintValidator : AbstractValidator<Domains.Sprint>
    {
        public SprintValidator()
        {
            // SprintName is required and max 300 chars
            RuleFor(x => x.SprintName)
                .NotEmpty().WithMessage("Sprint name is required.")
                .MaximumLength(300).WithMessage("Sprint name cannot exceed 300 characters.");

            // StartDate is required
            RuleFor(x => x.StartDate)
                .NotNull().WithMessage("Start date is required.");

            // EndDate is required and must be after StartDate
            RuleFor(x => x.EndDate)
                .NotNull().WithMessage("End date is required.")
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after the start date.");

            
            // DepartmentId must be selected (non-zero or positive)
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department is required.");

            // SprintGoal is optional but can add length restriction
            RuleFor(x => x.SprintGoal)
                .MaximumLength(2000).WithMessage("Sprint goal is too long.")
                .When(x => !string.IsNullOrWhiteSpace(x.SprintGoal));

            // BusinessGoal is optional but can add length restriction
            RuleFor(x => x.BusinessGoal)
                .MaximumLength(2000).WithMessage("Business goal is too long.")
                .When(x => !string.IsNullOrWhiteSpace(x.BusinessGoal));

            // DemoDate should be after StartDate, if both are set
            RuleFor(x => x.DemoDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("Demo date must be after the start date.")
                .When(x => x.StartDate.HasValue && x.DemoDate.HasValue);
        }
    }
}
