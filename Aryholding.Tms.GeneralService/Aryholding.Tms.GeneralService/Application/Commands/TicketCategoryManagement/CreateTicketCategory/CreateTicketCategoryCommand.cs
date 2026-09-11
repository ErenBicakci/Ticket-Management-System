using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.TicketCategoryManagement.CreateTicketCategory
{
    public record CreateTicketCategoryCommand(CreateTicketCategoryRequestDto Dto, string username) : IRequest<bool>
    {
    }
}
