namespace Aryholding.Tms.GeneralService.Domain.Constants
{
    public static class TicketEventTypeCodes
    {
        public const string Created = "CREATED";
        public const string StatusChanged = "STATUS_CHANGED";
        public const string PriorityChanged = "PRIORITY_CHANGED";
        public const string AssigneeChanged = "ASSIGNEE_CHANGED";
        public const string TitleChanged = "TITLE_CHANGED";
        public const string AttachmentAdded = "ATTACHMENT_ADDED";
        public const string AttachmentRemoved = "ATTACHMENT_REMOVED";
        public const string Reopened = "REOPENED";
        public const string SystemNote = "SYSTEM_NOTE";
        public const string Deleted = "DELETED";
        public const string Unassigned = "UNASSIGNED";
        public const string Updated = "UPDATED";
    }

}
