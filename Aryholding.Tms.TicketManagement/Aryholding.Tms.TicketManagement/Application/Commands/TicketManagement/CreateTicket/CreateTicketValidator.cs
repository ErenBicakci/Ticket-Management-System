using FluentValidation;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.CreateTicket
{
    public class CreateTicketValidator : AbstractValidator<CreateTicketCommand>
    {
        public CreateTicketValidator()
        {
            RuleFor(x => x.Dto.Title)
                .NotEmpty().WithMessage("Ticket başlığı zorunludur.")
                .Length(3, 200).WithMessage("Ticket başlığı 3-200 karakter arasında olmalıdır.");
            
            RuleFor(x => x.Dto.Description)
                .NotEmpty().WithMessage("Ticket açıklaması zorunludur.")
                .Length(10, 2000).WithMessage("Ticket açıklaması 10-2000 karakter arasında olmalıdır.");
            
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı zorunludur.");
            
            RuleFor(x => x.Dto.CategoryCode)
                .NotEmpty().WithMessage("Geçerli bir kategori kodu seçiniz.")
                .When(x => !string.IsNullOrEmpty(x.Dto.CategoryCode));
            
            RuleFor(x => x.Dto.SeverityCode)
                .NotEmpty().WithMessage("Geçerli bir önem seviyesi kodu seçiniz.")
                .When(x => !string.IsNullOrEmpty(x.Dto.SeverityCode));
        }
    }
}
