using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Notifications
{
    public class NewStoryNotification : Notification
    {
        public int StoryId { get; set; }

        public override string GetFormattedNotification()
        {
            return Message;
        }
    }
}
