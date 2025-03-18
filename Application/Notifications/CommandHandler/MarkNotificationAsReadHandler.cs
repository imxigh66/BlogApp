using Application.Abstractions;
using Application.Notifications.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications.CommandHandler
{
    public class MarkNotificationAsReadHandler : IRequestHandler<MarkNotificationAsRead, bool>
    {
        private readonly INotificationRepository _notificationRepository;

        public MarkNotificationAsReadHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> Handle(MarkNotificationAsRead request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.MarkAsReadAsync(request.NotificationId);
            return notification != null;
        }
    }
}
