using Aryholding.Tms.TicketManagement.Application.DTOs;

namespace Aryholding.Tms.TicketManagement.Application.Services.Interfaces
{
    public interface ITicketEventPublisher
    {
        Task PublishAsync(TicketEventMessage message, CancellationToken cancellationToken = default);
    }
}
