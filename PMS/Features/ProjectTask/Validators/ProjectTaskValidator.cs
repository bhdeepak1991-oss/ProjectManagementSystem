using FluentValidation;

namespace PMS.Features.ProjectTask.Validators
{
    public class ProjectTaskValidator : AbstractValidator<Domains.ProjectTask> // Replace with your class name
    {
        public ProjectTaskValidator()
        {
            // TaskName is required and should not exceed 200 characters
            RuleFor(x => x.TaskName)
                .NotEmpty().WithMessage("Task name is required.")
                .MaximumLength(200).WithMessage("Task name cannot exceed 200 characters.");

            // TaskDetail is optional but can be limited in length
            RuleFor(x => x.TaskDetail)
                .MaximumLength(2000).WithMessage("Task detail is too long.")
                .When(x => !string.IsNullOrWhiteSpace(x.TaskDetail));

            // TaskPriority (e.g., Low, Medium, High, Critical)
            RuleFor(x => x.TaskPriority)
                .NotEmpty().WithMessage("Task priority is required.")
                .Must(p => new[] { "Low", "Medium", "High", "Critical" }.Contains(p))
                .WithMessage("Task priority must be one of: Low, Medium, High, Critical.");

            // TaskType (e.g., Bug, Feature, Task)
            RuleFor(x => x.TaskType)
                .NotEmpty().WithMessage("Task type is required.")
                .Must(t => new[] { "Task", "Bug", "Feature" }.Contains(t))
                .WithMessage("Task type must be Task, Bug, or Feature.");

            // EmployeeId (Assignee)
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be valid.")
                .When(x => x.EmployeeId.HasValue);

            
            // ModuleName (optional)
            RuleFor(x => x.ModuleName)
                .MaximumLength(100).WithMessage("Module name can't exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.ModuleName));
        }
    }
}
