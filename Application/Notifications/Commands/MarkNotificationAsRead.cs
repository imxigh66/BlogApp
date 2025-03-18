﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications.Commands
{
    public class MarkNotificationAsRead : IRequest<bool>
    {
        public int NotificationId { get; set; }
    }
}
