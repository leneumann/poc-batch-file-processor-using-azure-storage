using System.Collections.Generic;
using System.Linq;

namespace Worker.Utils.Notification
{
    public class NotificationContext : INotificationContext
    {
        private readonly List<INotification> notifications;
        public bool HasNotifications => notifications.Any();


        public NotificationContext()
        {
            notifications = new List<INotification>();
        }
        public void Add(INotification notification)
        {
            notifications.Add(notification);
        }

        public void AddRange(List<INotification> notification)
        {
            notifications.AddRange(notification);
        }

        public IReadOnlyList<INotification> GetAll() => notifications;

    }
}
