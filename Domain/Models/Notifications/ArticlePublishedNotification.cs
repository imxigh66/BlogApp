using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Notifications
{
    public class ArticlePublishedNotification : Notification
    {
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }

        public override string GetFormattedNotification()
        {
            return $"Ваша статья \"{ArticleTitle}\" была опубликована.";
        }
    }
}
