using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Domain.Entities;

namespace Aryholding.Tms.TicketManagement.Application.Common.Mappers
{
    public static class TicketMapper
    {
        public static TicketResponseDto MapToResponseDto(Ticket ticket)
        {
            return new TicketResponseDto
            {
                TicketId = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Username = ticket.User?.UserName ?? string.Empty,
                AssignedUsername = ticket.AssignedUser?.UserName,
                PriorityLevel = ticket.PriorityLevel?.Level,
                CategoryCode = ticket.Category?.TicketCategoryCode,
                SeverityCode = ticket.Severity?.TicketSeverityCode,
                TicketStatusCode = ticket.TicketStatus?.TicketStatusCode,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            };
        }

        public static IEnumerable<TicketResponseDto> MapToResponseDtos(IEnumerable<Ticket> tickets)
        {
            return tickets.Select(MapToResponseDto);
        }
    }
}
