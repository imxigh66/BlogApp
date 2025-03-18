using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications.DTO
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string FormattedMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string NotificationType { get; set; }
    }
}
