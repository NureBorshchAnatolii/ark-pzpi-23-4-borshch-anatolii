using System.Reflection;
using CareLink.Application.Contracts.Repositories;
using CareLink.Application.Dtos.Messages;
using CareLink.Domain.Entities;

namespace CareLink.Application.Notifications
{
    public class NotificationDecorator<T> : DispatchProxy
    {
        private INotificationContentFactory _contentFactory = default!;
        private T _decorated = default!;
        private INotificationRepository _notificationRepository = default!;

        public void Init(
            T decorated,
            INotificationRepository repo,
            INotificationContentFactory factory)
        {
            _decorated = decorated;
            _notificationRepository = repo;
            _contentFactory = factory;
        }

        protected override object? Invoke(MethodInfo targetMethod, object[] args)
        {
            var attr = targetMethod.GetCustomAttribute<NotifyAttribute>();

            var returnType = targetMethod.ReturnType;

            if (typeof(Task).IsAssignableFrom(returnType)) return InvokeAsync(targetMethod, args, attr, returnType);

            // Синхронный метод
            var result = targetMethod.Invoke(_decorated, args);
            if (attr != null) CreateNotificationAsync(attr, args).GetAwaiter().GetResult();
            return result;
        }

        private object InvokeAsync(MethodInfo targetMethod, object[] args, NotifyAttribute? attr, Type returnType)
        {
            dynamic task = targetMethod.Invoke(_decorated, args)!;

            if (returnType == typeof(Task))
                // Метод возвращает Task
                return AwaitAndNotifyAsync(task, attr);

            // Метод возвращает Task<T>
            var resultType = returnType.GenericTypeArguments[0];
            return AwaitAndNotifyGenericAsync(task, attr, resultType);
        }

        private async Task AwaitAndNotifyAsync(Task task, NotifyAttribute? attr)
        {
            await task;
            if (attr != null)
                await CreateNotificationAsync(attr, task.AsyncState as object[] ?? Array.Empty<object>());
        }

        private object AwaitAndNotifyGenericAsync(dynamic task, NotifyAttribute? attr, Type resultType)
        {
            async Task<object> Wrapper()
            {
                var result = await task;
                if (attr != null)
                    await CreateNotificationAsync(attr, task.AsyncState as object[] ?? Array.Empty<object>());
                return result;
            }

            // Приведение к Task<T>
            var wrapperMethod = typeof(TaskExtensions)
                .GetMethod(nameof(TaskExtensions.CastTask), BindingFlags.Public | BindingFlags.Static)!
                .MakeGenericMethod(resultType);

            return wrapperMethod.Invoke(null, new object[] { Wrapper() })!;
        }

        private async Task CreateNotificationAsync(NotifyAttribute attr, object[] args)
        {
            if (args.FirstOrDefault() is not MessageCreateRequest r) return;

            var notification = new Notification
            {
                CreatedDate = DateTime.UtcNow,
                IsRead = false,
                UserId = r.ReceiverId,
                NotificationTypeId = attr.NotificationTypeId,
                Content = _contentFactory.Build(attr.NotificationTypeId, r),
                GroupOfIds = _contentFactory.BuildGroup(r)
            };

            await _notificationRepository.AddAsync(notification);
        }
    }
}