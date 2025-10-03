using FluentValidation;
using PMS.Domains;

namespace PMS.Features.Project.Validators
{
    public class ProjectValidator : AbstractValidator<Domains.Project>
    {
        public ProjectValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Project name is required.")
                .MaximumLength(100).WithMessage("Project name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
            RuleFor(x => x.ProjectStartDate)
                .LessThanOrEqualTo(x => x.ProjectEndDate).WithMessage("Start date must be before or equal to end date.")
                .When(x => x.ProjectEndDate.HasValue);
            RuleFor(x => x.ProjectEndDate)
                .GreaterThanOrEqualTo(x => x.ProjectStartDate).WithMessage("End date must be after or equal to start date.")
                .When(x => x.ProjectStartDate.HasValue);
            RuleFor(x => x.ProjectManager)
                .NotNull().WithMessage("Project Manager is required.")
                .GreaterThan(0).WithMessage("Project Manager must be a valid ID.");

            RuleFor(x => x.ProjectHead)
                .NotNull().WithMessage("Project Head is required.")
                .GreaterThan(0).WithMessage("Project Head must be a valid ID.");

            RuleFor(x => x.DeliveryHead)
                .NotNull().WithMessage("Delivery Head is required.")
                .GreaterThan(0).WithMessage("Delivery Head must be a valid ID.");


        }
    }
}
