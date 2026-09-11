
using Aryholding.Tms.TicketManagement.Application.Common.Mappers;
using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Application.Services.Interfaces;
using Aryholding.Tms.TicketManagement.Domain.Constants;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.UpdateTicket
{
    public class UpdateTicketHandler : IRequestHandler<UpdateTicketCommand, TicketResponseDto>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITicketEventService _ticketEventService;
        public UpdateTicketHandler(ITicketRepository ticketRepository, IUserRepository userRepository, ITicketEventService ticketEventService)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _ticketEventService = ticketEventService;
        }



        public async Task<TicketResponseDto> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userRepository.GetUserIdByUsernameAsync(request.Username);

            var ticket = await _ticketRepository.GetByIdAsync(request.Dto.TicketId);
            if (ticket == null || !ticket.IsActive)
            {
                throw new KeyNotFoundException($"Ticket with ID {request.Dto.TicketId} not found or is inactive.");
            }

            var userHasRole  = await _userRepository.UserHasRoleInDepartmentAsync(userId, ticket.PriorityLevel.Level,ticket.Category.DepartmentId);

            if (!userHasRole && ticket.UserId != userId)
            {
                throw new UnauthorizedAccessException("User does not have the required role to update this ticket.");
            }

            ticket.Title = request.Dto.Title;
            ticket.Description = request.Dto.Description;
            ticket.UpdatedAt = DateTime.UtcNow;
            _ticketRepository.Update(ticket);
            await _ticketRepository.SaveChangesAsync();

            await _ticketEventService.AddEventAsync(ticket.Id, userId, TicketEventTypeCodes.Updated, $"Ticket Updated {request.Username}");
            return TicketMapper.MapToResponseDto(ticket);
        }
    }
}
