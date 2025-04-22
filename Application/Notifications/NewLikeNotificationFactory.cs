using Domain.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications
{
    public class NewLikeNotificationFactory:INotificationFactory
    {
        private readonly int _userId;
        private readonly int _articleId;
        private readonly int _likeId;

        public NewLikeNotificationFactory(int userId,int articleId,int likeId)
        {
            _articleId = articleId;
            _userId = userId;
            _likeId = likeId;
        }

        public Notification CreateNotification()
        {
            return new LikeNotification
            {
                UserId = _userId,
                ArticleId = _articleId,
                LikeId = _likeId,
                Message = "Новый лайк добавлен  к вашей статье"
            };
        }
    }
}
