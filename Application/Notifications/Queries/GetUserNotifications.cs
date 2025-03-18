using Application.Notifications.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications.Queries
{
    public class GetUserNotifications : IRequest<List<NotificationDto>>
    {
        public int UserId { get; set; }
    }
}
