using Aryholding.Tms.GeneralService.Application.DTOs;
using Aryholding.Tms.GeneralService.Domain.Entities;

namespace Aryholding.Tms.GeneralService.Application.Common.Mappers
{
    public static class TicketCategoryMapper
    {
        public static TicketCategoryResponseDTO MapToResponseDto(TicketCategory ticketCategory)
        {
            return new TicketCategoryResponseDTO
            {
                Code = ticketCategory.TicketCategoryCode,
                Name = ticketCategory.Name
            };
        }

        public static IEnumerable<TicketCategoryResponseDTO> MapToResponseDtos(IEnumerable<TicketCategory> ticketCategories)
        {
            return ticketCategories.Select(MapToResponseDto);
        }
    }
}
