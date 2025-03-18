﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Notifications
{
    public abstract class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        public int UserId { get; set; }

        // Добавьте поле для идентификации типа уведомления
        public string NotificationType { get; set; }

        // Изменить с abstract на virtual
        public virtual string GetFormattedNotification()
        {
            return Message;
        }
    }
}
