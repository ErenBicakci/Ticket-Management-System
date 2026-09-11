using FluentValidation;

namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.UpdateDepartmentRoles
{
    public class UpdateDepartmentRolesValidator : AbstractValidator<UpdateDepartmentRolesCommand>
    {
        public UpdateDepartmentRolesValidator()
        {
            RuleFor(x => x.Dto.UserUsername)
                .NotEmpty().WithMessage("User username is required.");

            RuleFor(x => x.Dto.PriorityLevel)
                .InclusiveBetween(1, 10).WithMessage("Priority level must be between 1 and 10.");
        }
    }
}
