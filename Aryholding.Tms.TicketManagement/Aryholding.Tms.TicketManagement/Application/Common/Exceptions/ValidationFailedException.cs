using FluentValidation.Results;

namespace Aryholding.Tms.TicketManagement.Application.Common.Exceptions
{
    public class ValidationFailedException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationFailedException() 
            : base("Bir veya daha fazla doğrulama hatası oluştu.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationFailedException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidationFailedException(string propertyName, string errorMessage)
            : this()
        {
            Errors = new Dictionary<string, string[]>
            {
                { propertyName, new[] { errorMessage } }
            };
        }

        public ValidationFailedException(IDictionary<string, string[]> errors)
            : this()
        {
            Errors = errors;
        }

        public IDictionary<string, string[]> GetErrorDictionary()
        {
            return Errors;
        }
    }
}
