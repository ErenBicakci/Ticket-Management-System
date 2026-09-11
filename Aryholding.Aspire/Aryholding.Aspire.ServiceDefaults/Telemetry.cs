using System.Diagnostics;

namespace Microsoft.Extensions.Hosting;

public static class Telemetry
{
    public const string BusinessSourceName = "Aryholding.Tms.BusinessOperations";
    public const string MessagingSourceName = "Aryholding.Tms.Messaging";

    public static readonly ActivitySource BusinessSource = new(BusinessSourceName);
    public static readonly ActivitySource MessagingSource = new(MessagingSourceName);
}
