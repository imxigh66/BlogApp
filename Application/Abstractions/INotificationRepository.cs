using Domain.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface INotificationRepository
    {
        Task<ICollection<Notification>> GetNotificationsByUserIdAsync(int userId);
        Task<Notification> AddNotificationAsync(Notification notification);
        Task<Notification> MarkAsReadAsync(int notificationId);
    }
}
