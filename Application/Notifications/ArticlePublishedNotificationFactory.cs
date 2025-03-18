using Domain.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications
{
    public class ArticlePublishedNotificationFactory : INotificationFactory
    {
        private readonly int _userId;
        private readonly int _articleId;
        private readonly string _articleTitle;

        public ArticlePublishedNotificationFactory(int userId, int articleId, string articleTitle)
        {
            _userId = userId;
            _articleId = articleId;
            _articleTitle = articleTitle;
        }

        public Notification CreateNotification()
        {
            return new ArticlePublishedNotification
            {
                UserId = _userId,
                ArticleId = _articleId,
                ArticleTitle = _articleTitle,
                Message = "Ваша статья была опубликована"
            };
        }
    }
}
