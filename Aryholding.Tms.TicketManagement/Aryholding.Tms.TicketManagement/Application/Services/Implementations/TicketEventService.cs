using Aryholding.Tms.TicketManagement.Application.Common.Exceptions;
using Aryholding.Tms.TicketManagement.Application.Services.Interfaces;
using Aryholding.Tms.TicketManagement.Domain.Entities;
using Aryholding.Tms.TicketManagement.Infrastructure.Data;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.TicketManagement.Application.Services.Implementationts
{
    public class TicketEventService : ITicketEventService
    {

        private readonly ITicketHistoryRepository _ticketHistoryRepository;
        private readonly TicketDbContext _context;

        public TicketEventService(ITicketHistoryRepository ticketEventRepository,TicketDbContext corsDb)
        {
            _ticketHistoryRepository = ticketEventRepository;
            _context = corsDb;
        }

        public async Task AddEventAsync(long ticketId, string userId, string eventTypeCode, string message)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) {
                throw new NotFoundException("User", userId);
            }
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) {
                throw new NotFoundException("Ticket", ticketId);
            }

            var eventType = await _context.TicketEventTypes.
                Where(et => et.EventTypeCode == eventTypeCode).
                FirstOrDefaultAsync();
            if (eventType == null) {
                throw new NotFoundException("TicketEventType", eventTypeCode);
            }

            var history = new TicketHistory
            {
                TicketId = ticketId,
                ChangedByUserId = userId,
                ChangedByUser = user,
                TicketEventTypeId = eventType.Id,
                TicketEventType = eventType,
                ChangedAt = DateTime.UtcNow,
                EventMessage = message
            };

            await _ticketHistoryRepository.AddAsync(history);
            await _ticketHistoryRepository.SaveChangesAsync();
        }

        public async Task AddStatusChangeAsync(long ticketId, string userId, int? oldStatusId, int newStatusId, string message)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User", userId);
            }
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null)
            {
                throw new NotFoundException("Ticket", ticketId);
            }

            var eventType = await _context.TicketEventTypes.
                Where(et => et.EventTypeCode == "STATUS_CHANGED").
                FirstOrDefaultAsync();
            if (eventType == null)
            {
                throw new NotFoundException("TicketEventType", "STATUS_CHANGED");
            }

            var history = new TicketHistory
            {
                TicketId = ticketId,
                ChangedByUserId = userId,
                ChangedByUser = user,
                TicketEventTypeId = eventType.Id,
                TicketEventType = eventType,
                ChangedAt = DateTime.UtcNow,
                EventMessage = message,
                OldStatusId = oldStatusId,
                NewStatusId = newStatusId
            };

            await _ticketHistoryRepository.AddAsync(history);
            await _ticketHistoryRepository.SaveChangesAsync();
        }
    }
}
