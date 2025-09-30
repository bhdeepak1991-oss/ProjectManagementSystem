using FluentValidation;
using PMS.Domains;

namespace PMS.Features.Master.Validators
{
    public class RoleValidator : AbstractValidator<RoleMaster>
    {
        public RoleValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Role Name is required.");
        }
    }
}
