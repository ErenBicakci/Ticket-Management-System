using FluentValidation;

namespace Aryholding.Tms.GeneralService.Application.Commands.TicketCategoryManagement.CreateTicketCategory
{
    public class CreateTicketCategoryValidator : AbstractValidator<CreateTicketCategoryCommand>
    {
        public CreateTicketCategoryValidator()
        {
            RuleFor(x => x.Dto.CategoryName)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

            RuleFor(x => x.Dto.CategoryCode)
                .NotEmpty().WithMessage("Category code is required.")
                .MaximumLength(20).WithMessage("Category code must not exceed 20 characters.");

            RuleFor(x => x.Dto.DepartmentCode)
                .NotEmpty().WithMessage("Department code is required.");
        }
    }
}
