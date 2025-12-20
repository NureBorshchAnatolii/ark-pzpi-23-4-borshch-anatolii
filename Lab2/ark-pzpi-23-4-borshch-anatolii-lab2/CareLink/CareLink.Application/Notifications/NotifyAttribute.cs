namespace CareLink.Application.Notifications
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class NotifyAttribute : Attribute
    {
        public long NotificationTypeId { get; }

        public NotifyAttribute(long notificationTypeId)
        {
            NotificationTypeId = notificationTypeId;
        }
    }
}