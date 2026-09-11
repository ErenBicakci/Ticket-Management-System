namespace Aryholding.Tms.AuthService.Application.Common.Exceptions
{
    public class BusinessException : Exception
    {
        public string ErrorCode { get; }
        public object? Details { get; }

        public BusinessException(string message, string? errorCode = null, object? details = null) 
            : base(message)
        {
            ErrorCode = errorCode ?? "BUSINESS_ERROR";
            Details = details;
        }

        public BusinessException(string message, Exception innerException, string? errorCode = null, object? details = null) 
            : base(message, innerException)
        {
            ErrorCode = errorCode ?? "BUSINESS_ERROR";
            Details = details;
        }
    }
}
