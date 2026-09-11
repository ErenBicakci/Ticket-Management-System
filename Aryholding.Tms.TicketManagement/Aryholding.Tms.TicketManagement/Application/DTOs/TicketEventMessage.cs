namespace Aryholding.Tms.TicketManagement.Application.DTOs
{
    public sealed record TicketEventMessage(
        Guid EventId,
        string EventType,
        long TicketId,
        string Actor,
        string Message,
        DateTime OccurredAtUtc);
}
