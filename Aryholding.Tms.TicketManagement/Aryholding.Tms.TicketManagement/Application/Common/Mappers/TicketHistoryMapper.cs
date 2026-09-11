using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Domain.Entities;

namespace Aryholding.Tms.TicketManagement.Application.Common.Mappers
{
    public class TicketHistoryMapper
    {

        public static TicketHistoryResponseDto MapToResponseDto(TicketHistory ticketHistories)
        {
            return new TicketHistoryResponseDto
            {
                EventCode = ticketHistories.TicketEventType.EventTypeCode,
                EventMessage = ticketHistories.EventMessage,
                Username = ticketHistories.ChangedByUser.UserName
            };
        }

        public static IEnumerable<TicketHistoryResponseDto> MapToResponseDtos(IEnumerable<TicketHistory> ticketHistories)
        {
            return ticketHistories.Select(MapToResponseDto);
        }
    }
}
