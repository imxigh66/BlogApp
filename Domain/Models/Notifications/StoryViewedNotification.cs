using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Notifications
{
    public class StoryViewedNotification : Notification
    {
        public int StoryId { get; set; }
        public int ViewerId { get; set; }

        public override string GetFormattedNotification()
        {
            return Message;
        }
    }
}
