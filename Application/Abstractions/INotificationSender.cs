using Domain.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface INotificationSender
    {
        Task SendAsync(Notification notification);
    }
}
