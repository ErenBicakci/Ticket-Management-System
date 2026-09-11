using FluentValidation;

namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.CreateDepartment
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentCommand>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.dto.DepartmentName)
                .NotEmpty().WithMessage("Department name is required.")
                .MaximumLength(100).WithMessage("Department name must not exceed 100 characters.");

            RuleFor(x => x.dto.DepartmentCode)
                .NotEmpty().WithMessage("Department code is required.")
                .MaximumLength(20).WithMessage("Department code must not exceed 20 characters.");
        }
    }
}
