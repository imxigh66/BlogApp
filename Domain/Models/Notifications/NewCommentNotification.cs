using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Notifications
{
    public class NewCommentNotification : Notification
    {
        public int ArticleId { get; set; }
        public int CommentId { get; set; }
        public string CommenterName { get; set; }

        public override string GetFormattedNotification()
        {
            return $"{CommenterName} оставил(а) комментарий к вашей статье.";
        }
    }
}
