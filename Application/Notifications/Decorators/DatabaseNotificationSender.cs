using Application.Abstractions;
using Domain.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications.Decorators
{
    public class DatabaseNotificationSender : INotificationSender
    {
        private readonly INotificationRepository _repository;

        public DatabaseNotificationSender(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task SendAsync(Notification notification)
        {
            await _repository.AddNotificationAsync(notification);
        }
    }
}
