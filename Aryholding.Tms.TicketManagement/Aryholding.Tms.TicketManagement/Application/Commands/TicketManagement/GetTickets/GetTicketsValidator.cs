using FluentValidation;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.GetTickets
{
    public class GetTicketsValidator : AbstractValidator<GetTicketsCommand>
    {
        public GetTicketsValidator()
        {
            RuleFor(x => x.SeverityCode)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.SeverityCode))
                .WithMessage("Severity code cannot exceed 50 characters");

            RuleFor(x => x.StatusCode)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.StatusCode))
                .WithMessage("Status code cannot exceed 50 characters");

            RuleFor(x => x.CategoryCode)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.CategoryCode))
                .WithMessage("Category code cannot exceed 50 characters");

            RuleFor(x => x.Username)
                .MaximumLength(256)
                .When(x => !string.IsNullOrEmpty(x.Username))
                .WithMessage("Username cannot exceed 256 characters");
        }
    }
}
