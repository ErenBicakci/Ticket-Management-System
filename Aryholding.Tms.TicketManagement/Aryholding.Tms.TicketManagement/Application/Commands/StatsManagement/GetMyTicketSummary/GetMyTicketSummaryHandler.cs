using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.StatsManagement.GetMyTicketSummary
{
    public class GetMyTicketSummaryHandler : IRequestHandler<GetMyTicketSummaryQuery, UserTicketSummaryDto>
    {
        private readonly ITicketRepository _ticketRepository;

        public GetMyTicketSummaryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<UserTicketSummaryDto> Handle(GetMyTicketSummaryQuery request, CancellationToken cancellationToken)
        {
            return await _ticketRepository.GetUserTicketSummaryAsync(request.Username, cancellationToken);
        }
    }
}
