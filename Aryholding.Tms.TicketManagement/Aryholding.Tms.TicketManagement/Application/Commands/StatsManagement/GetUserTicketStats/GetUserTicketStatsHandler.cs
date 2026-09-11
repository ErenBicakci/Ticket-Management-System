using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.StatsManagement.GetUserTicketStats
{
    public class GetUserTicketStatsHandler : IRequestHandler<GetUserTicketStatsCommand, List<UserTicketStatsDto>>
    {
        private readonly ITicketHistoryRepository _ticketHistoryRepository;

        public GetUserTicketStatsHandler(ITicketHistoryRepository ticketHistoryRepository)
        {
            _ticketHistoryRepository = ticketHistoryRepository;
        }

        public async Task<List<UserTicketStatsDto>> Handle(GetUserTicketStatsCommand request, CancellationToken cancellationToken)
        {
            return await _ticketHistoryRepository.GetUserTicketStatsAsync(
                request.FromUtc,
                request.ToUtc,
                request.DepartmentCode,
                cancellationToken);
        }
    }
}
