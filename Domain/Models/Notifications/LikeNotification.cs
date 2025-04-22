using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Notifications
{
    public class LikeNotification:Notification
    {
        public int ArticleId { get; set; }
        public int LikeId { get; set; }

        
    }
}
