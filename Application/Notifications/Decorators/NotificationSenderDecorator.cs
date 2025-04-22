using Application.Abstractions;
using Domain.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications.Decorators
{
    public abstract class NotificationSenderDecorator : INotificationSender
    {
        protected readonly INotificationSender _wrappedSender;

        protected NotificationSenderDecorator(INotificationSender wrappedSender)
        {
            _wrappedSender = wrappedSender;
        }

        public virtual async Task SendAsync(Notification notification)
        {
            await _wrappedSender.SendAsync(notification);
        }
    }
}
