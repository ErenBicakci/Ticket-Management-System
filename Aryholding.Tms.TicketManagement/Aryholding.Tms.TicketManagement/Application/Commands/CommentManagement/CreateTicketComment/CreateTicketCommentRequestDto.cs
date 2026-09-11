namespace Aryholding.Tms.TicketManagement.Application.Commands.CommentManagement.CreateTicketComment
{
    public class CreateTicketCommentRequestDto
    {
        public long ticketId {  get; set; }
        public string comment {  get; set; }
    }
}
