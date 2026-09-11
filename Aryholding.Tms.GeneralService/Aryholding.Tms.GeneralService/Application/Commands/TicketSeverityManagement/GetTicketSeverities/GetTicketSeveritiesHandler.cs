using Aryholding.Tms.GeneralService.Application.Common.Mappers;
using Aryholding.Tms.GeneralService.Application.DTOs;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.TicketSeverityManagement.GetTicketSeverities
{
    public class GetTicketSeveritiesHandler : IRequestHandler<GetTicketSeveritiesCommand, IEnumerable<TicketSeverityResponseDTO>>
    {
        private readonly ITicketSeverityRepository _ticketSeverityRepository;

        public GetTicketSeveritiesHandler(ITicketSeverityRepository ticketSeverityRepository)
        {
            _ticketSeverityRepository = ticketSeverityRepository;
        }

        public async Task<IEnumerable<TicketSeverityResponseDTO>> Handle(GetTicketSeveritiesCommand request, CancellationToken cancellationToken)
        {
            var ticketSeverities = await _ticketSeverityRepository.GetAllAsync();
            return TicketSeverityMapper.MapToResponseDtos(ticketSeverities);
        }
    }
}
