using Application.Abstractions;
using Application.Notifications.DTO;
using Application.Notifications.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications.QueryHandlers
{
    public class GetUserNotificationsHandler : IRequestHandler<GetUserNotifications, List<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetUserNotificationsHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<List<NotificationDto>> Handle(GetUserNotifications request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(request.UserId);

            return notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                FormattedMessage = n.GetFormattedNotification(),
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead,
                NotificationType = n.NotificationType
            }).ToList();
        }
    }
}
