
using Aryholding.Tms.TicketManagement.Application.Common.Exceptions;
using Aryholding.Tms.TicketManagement.Application.Common.Mappers;
using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Application.Services.Interfaces;
using Aryholding.Tms.TicketManagement.Domain.Constants;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.AssignTicket
{
    public class AssignTicketHandler : IRequestHandler<AssignTicketCommand, TicketResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketEventService _ticketEventService;
        private readonly ITicketStatusRepository _ticketStatusRepository;

        public AssignTicketHandler(IUserRepository userRepository, ITicketRepository ticketRepository, ITicketEventService ticketEventService,ITicketStatusRepository ticketStatusRepository)
        {
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _ticketEventService = ticketEventService;
            _ticketStatusRepository = ticketStatusRepository;
        }

        public async Task<TicketResponseDto> Handle(AssignTicketCommand request, CancellationToken cancellationToken)
        {
            var assignedUserId = await _userRepository.GetUserIdByUsernameAsync(request.assignedUser);
            if(assignedUserId == null)
            {
                throw new ArgumentException("Kullanıcı mevcut değil");
            }

            var assignerUserId = await _userRepository.GetUserIdByUsernameAsync(request.assignerUser);
            var ticket = await _ticketRepository.GetByIdAsync(request.ticketId);
            if (ticket == null)
            {
                throw new ArgumentException("Talep mevcut değil");
            }
             
            var assignerUserHasRole = await _userRepository.UserHasRoleInDepartmentAsync(assignerUserId, 0, ticket.Category.DepartmentId);
            
            if(!assignerUserHasRole)
            {
                throw new UnauthorizedAccessException("User does not have access to this department's tickets.");
            }

            var assignedUserHasRole = await _userRepository.UserHasRoleInDepartmentAsync(assignedUserId, 0, ticket.Category.DepartmentId);
            if (!assignedUserHasRole)
            {
                throw new BusinessException("Atanmak istenen kullanıcı bu talebin bulunduğu departmanda değil");
            }



            if (ticket.TicketStatus.TicketStatusCode == TicketStatusCodes.WaitingApproval || ticket.TicketStatus.TicketStatusCode == TicketStatusCodes.Approved)
            {
                throw new BusinessException("You cannot assign a ticket that is WaitingApproval or Approved.");
            }

            int? oldStatusId = ticket.TicketStatusId;
            ticket.AssignedUserId = assignedUserId;
            var ticketStatus = await _ticketStatusRepository.GetByCodeAsync(TicketStatusCodes.Accepted);
            int newStatusId = ticketStatus.Id;
            ticket.TicketStatusId = newStatusId;

            await using (var transaction = await _ticketRepository.BeginTransactionAsync())
            {
                try
                {
                    _ticketRepository.Update(ticket);
                    await _ticketRepository.SaveChangesAsync();

                    await _ticketEventService.AddEventAsync(ticket.Id, assignerUserId, TicketEventTypeCodes.AssigneeChanged, $"Ticket Assigned {request.assignedUser} Ticket Assigner {request.assignerUser}");
                    await _ticketEventService.AddStatusChangeAsync(ticket.Id, assignerUserId, oldStatusId, newStatusId, $"Status changed to Accepted by assignment to {request.assignedUser}");

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return TicketMapper.MapToResponseDto(ticket);
        }
    }
}
