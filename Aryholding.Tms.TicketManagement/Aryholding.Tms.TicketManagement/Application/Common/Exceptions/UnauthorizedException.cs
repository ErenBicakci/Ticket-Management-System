namespace Aryholding.Tms.TicketManagement.Application.Common.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() 
            : base("Bu işlemi gerçekleştirmek için yetkiniz bulunmamaktadır.")
        {
        }

        public UnauthorizedException(string message) 
            : base(message)
        {
        }

        public UnauthorizedException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
