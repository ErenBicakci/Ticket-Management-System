using Aryholding.Tms.GeneralService.Application.DTOs;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.TicketCategoryManagement.GetTicketCategories
{
    public record GetTicketCategoriesCommand(string? departmentCode) : IRequest<IEnumerable<TicketCategoryResponseDTO>>
    {
    }
}
