using FluentValidation.Results;

namespace Aryholding.Tms.AuthService.Application.Common.Exceptions
{
    public class ValidationFailedException : Exception
    {
        public List<ValidationFailure> Errors { get; }

        public ValidationFailedException(List<ValidationFailure> errors) 
            : base("One or more validation failures have occurred.")
        {
            Errors = errors;
        }

        public Dictionary<string, string[]> GetErrorDictionary()
        {
            return Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray()
                );
        }
    }
}
