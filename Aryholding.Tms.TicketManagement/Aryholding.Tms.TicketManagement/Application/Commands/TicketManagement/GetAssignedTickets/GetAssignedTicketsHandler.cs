
using Aryholding.Tms.TicketManagement.Application.Common.Mappers;
using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.GetAssignedTickets
{
    public class GetAssignedTicketsHandler : IRequestHandler<GetAssignedTicketsCommand, IEnumerable<TicketResponseDto>>
    {
        private readonly ITicketRepository _ticketRepository;

        public GetAssignedTicketsHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<IEnumerable<TicketResponseDto>> Handle(GetAssignedTicketsCommand request, CancellationToken cancellationToken)
        {
            var tickets = await _ticketRepository.GetAssignedTicketsWithFiltersAsync(
                request.SeverityCode,
                request.StatusCode,
                request.CategoryCode,
                request.Username,
                request.OrderDirection,
                request.Page,
                10
            );

            return TicketMapper.MapToResponseDtos(tickets);
        }
    }
}
