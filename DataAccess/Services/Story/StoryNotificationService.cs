using Application.Abstractions;
using Domain.Models;
using Domain.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.Story
{
    public class StoryNotificationService : IStoryNotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public StoryNotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task NotifyNewStoryAsync(Domain.Models.Story story)
        {
            // В реальном приложении здесь может быть логика отправки пуш-уведомлений
            // для подписчиков пользователя

            // Для демонстрации просто создаем запись в таблице уведомлений
            var notification = new NewStoryNotification
            {
                Message = $"Пользователь {story.Author.Username} опубликовал новую историю",
                CreatedAt = DateTime.UtcNow,
                NotificationType = "NewStory",
                UserId = 1 // Заглушка
            };

            await _notificationRepository.AddNotificationAsync(notification);
        }

        public async Task NotifyStoryViewedAsync(StoryView view)
        {
            // Уведомляем автора истории о просмотре
            var notification = new StoryViewedNotification
            {
                Message = $"Ваша история была просмотрена пользователем ID: {view.ViewerId}",
                CreatedAt = DateTime.UtcNow,
                NotificationType = "StoryViewed",
                UserId = view.Story.AuthorId
            };

            await _notificationRepository.AddNotificationAsync(notification);
        }
    }
}
