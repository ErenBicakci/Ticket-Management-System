
using Aryholding.Tms.TicketManagement.Application.Common.Exceptions;
using Aryholding.Tms.TicketManagement.Application.Services.Interfaces;
using Aryholding.Tms.TicketManagement.Domain.Constants;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.RejectTicket
{
    public class RejectTicketHandler : IRequestHandler<RejectTicketCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketStatusRepository _ticketStatusRepository;
        private readonly ITicketEventService _ticketEventService;

        public RejectTicketHandler(IUserRepository userRepository, ITicketRepository ticketRepository, ITicketStatusRepository ticketStatusRepository, ITicketEventService ticketEventService)
        {
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _ticketStatusRepository = ticketStatusRepository;
            _ticketEventService = ticketEventService;
        }

        public async Task<bool> Handle(RejectTicketCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userRepository.GetUserIdByUsernameAsync(request.username);

            var ticket = await _ticketRepository.GetByIdAsync(request.ticketId);

            var userHasRole = await _userRepository.UserHasRoleInDepartmentAsync(userId, 7, ticket.Category.DepartmentId);

            if (!userHasRole)
            {
                throw new UnauthorizedException("User does not have the required role to reject this ticket.");
            }

            if (ticket.TicketStatus.TicketStatusCode != TicketStatusCodes.WaitingApproval)
            {
                throw new InvalidOperationException("Ticket status exception");
            }

            int? oldStatusId = ticket.TicketStatusId;
            int newStatusId = (await _ticketStatusRepository.GetIdByCodeAsync(TicketStatusCodes.Accepted)) ?? 0;
            ticket.TicketStatusId = newStatusId;

            await using (var transaction = await _ticketRepository.BeginTransactionAsync())
            {
                try
                {
                    await _ticketRepository.SaveChangesAsync();
                    await _ticketEventService.AddStatusChangeAsync(ticket.Id, userId, oldStatusId, newStatusId, $"Ticket Rejected {request.username}");
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
