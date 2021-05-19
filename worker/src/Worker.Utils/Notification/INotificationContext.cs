using System.Collections.Generic;

namespace Worker.Utils.Notification
{
    public interface INotificationContext
    {
        bool HasNotifications { get; }

        void Add(INotification notification);

        IReadOnlyList<INotification> GetAll();

    }
}
