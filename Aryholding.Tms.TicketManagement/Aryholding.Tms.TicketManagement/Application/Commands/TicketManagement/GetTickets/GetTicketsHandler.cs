using Aryholding.Tms.TicketManagement.Application.Common.Mappers;
using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.GetTickets
{
    public class GetTicketsHandler : IRequestHandler<GetTicketsCommand, IEnumerable<TicketResponseDto>>
    {
        private readonly ITicketRepository _ticketRepository;

        public GetTicketsHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<IEnumerable<TicketResponseDto>> Handle(GetTicketsCommand request, CancellationToken cancellationToken)
        {
            var tickets = await _ticketRepository.GetTicketsWithFiltersAsync(
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
