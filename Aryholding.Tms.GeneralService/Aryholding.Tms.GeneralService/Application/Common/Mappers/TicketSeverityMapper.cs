using Aryholding.Tms.GeneralService.Application.DTOs;
using Aryholding.Tms.GeneralService.Domain.Entities;

namespace Aryholding.Tms.GeneralService.Application.Common.Mappers
{
    public static class TicketSeverityMapper
    {
        public static TicketSeverityResponseDTO MapToResponseDto(TicketSeverity ticketSeverity)
        {
            return new TicketSeverityResponseDTO
            {
                Code = ticketSeverity.TicketSeverityCode,
                Name = ticketSeverity.Name
            };
        }

        public static IEnumerable<TicketSeverityResponseDTO> MapToResponseDtos(IEnumerable<TicketSeverity> ticketSeverities)
        {
            return ticketSeverities.Select(MapToResponseDto);
        }
    }
}
