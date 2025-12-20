using System.Reflection;
using CareLink.Application.Contracts.Repositories;

namespace CareLink.Application.Notifications
{
    public static class NotificationDecoratorFactory
    {
        public static T Create<T>(
            T decorated,
            INotificationRepository repo,
            INotificationContentFactory factory)
        {
            var proxy = DispatchProxy.Create<T, NotificationDecorator<T>>();
            ((NotificationDecorator<T>)(object)proxy).Init(decorated, repo, factory);
            return proxy;
        }
    }
}