namespace Aryholding.Tms.AuthService.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public string ResourceName { get; }
        public object ResourceKey { get; }

        public NotFoundException(string resourceName, object resourceKey) 
            : base($"{resourceName} with key '{resourceKey}' was not found.")
        {
            ResourceName = resourceName;
            ResourceKey = resourceKey;
        }

        public NotFoundException(string message) : base(message)
        {
            ResourceName = "Resource";
            ResourceKey = "Unknown";
        }
    }
}
