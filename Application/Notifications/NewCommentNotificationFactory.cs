using Domain.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications
{
    public class NewCommentNotificationFactory : INotificationFactory
    {
        private readonly int _userId;
        private readonly int _articleId;
        private readonly int _commentId;
        private readonly string _commenterName;

        public NewCommentNotificationFactory(int userId, int articleId, int commentId, string commenterName)
        {
            _userId = userId;
            _articleId = articleId;
            _commentId = commentId;
            _commenterName = commenterName;
        }

        public Notification CreateNotification()
        {
            return new NewCommentNotification
            {
                UserId = _userId,
                ArticleId = _articleId,
                CommentId = _commentId,
                CommenterName = _commenterName,
                Message = "Новый комментарий к вашей статье"
            };
        }
    }
}
