
using Aryholding.Tms.TicketManagement.Application.Common.Exceptions;
using Aryholding.Tms.TicketManagement.Application.Services.Interfaces;
using Aryholding.Tms.TicketManagement.Domain.Constants;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.SubmitTicketForApproval
{
    public class SubmitTicketForApprovalHandler : IRequestHandler<SubmitTicketForApprovalCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketStatusRepository _ticketStatusRepository;
        private readonly ITicketEventService _ticketEventService;

        public SubmitTicketForApprovalHandler(IUserRepository userRepository,
            ITicketRepository ticketRepository,
            ITicketStatusRepository ticketStatusRepository,
            ITicketEventService ticketEventService)
        {
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _ticketStatusRepository = ticketStatusRepository;
            _ticketEventService = ticketEventService;
        }

        public async Task<bool> Handle(SubmitTicketForApprovalCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userRepository.GetUserIdByUsernameAsync(request.username);
            var ticket = await _ticketRepository.GetByIdAsync(request.ticketId);
            if (userId == null || ticket == null)
            {
                throw new NotFoundException("User or ticket not found.");
            }
            var userHasRole = await _userRepository.UserHasRoleInDepartmentAsync(userId, 2, ticket.Category.DepartmentId);
            if (!userHasRole)
            {
                throw new UnauthorizedException("User does not have the required role to submit this ticket for approval.");
            }
            if(userId != ticket.AssignedUserId)
            {
                throw new UnauthorizedException("Only the assigned user can submit the ticket for approval.");
            }
            if(ticket.TicketStatus.TicketStatusCode != TicketStatusCodes.Accepted)
            {
                throw new InvalidOperationException("Only tickets with 'In Progress' status can be submitted for approval.");
            }

            int? oldStatusId = ticket.TicketStatusId;
            int newStatusId = (await _ticketStatusRepository.GetIdByCodeAsync(TicketStatusCodes.WaitingApproval)) ?? 0;
            ticket.TicketStatusId = newStatusId;

            await using (var transaction = await _ticketRepository.BeginTransactionAsync())
            {
                try
                {
                    await _ticketRepository.SaveChangesAsync();
                    await _ticketEventService.AddStatusChangeAsync(ticket.Id, userId, oldStatusId, newStatusId, $"Submited Ticket For Approval {request.username}");
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return true;
        }
    }
}
