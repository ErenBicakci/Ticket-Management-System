namespace Aryholding.Tms.TicketManagement.Application.Services.Interfaces
{
    public interface ITicketEventService
    {
        Task AddEventAsync(long ticketId, string userId, string eventTypeCode, string eventMessage);

        Task AddStatusChangeAsync(
            long ticketId,
            string userId,
            int? oldStatusId,
            int newStatusId,
            string message);
    }
}
