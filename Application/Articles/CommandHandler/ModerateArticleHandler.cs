// Application/Articles/CommandHandler/ModerateArticleHandler.cs
using Application.Abstractions;
using Application.Articles.Command;
using Application.Notifications;
using Domain.States;
using MediatR;

namespace Application.Articles.CommandHandler
{
    public class ModerateArticleHandler : IRequestHandler<ModerateArticleCommand, ArticleResult>
    {
        private readonly IArticleRepository _repository;
        private readonly INotificationRepository _notificationRepository;

        public ModerateArticleHandler(IArticleRepository repository, INotificationRepository notificationRepository)
        {
            _repository = repository;
            _notificationRepository = notificationRepository;
        }

        public async Task<ArticleResult> Handle(ModerateArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _repository.GetArticleById(request.ArticleId);
            if (article == null)
            {
                return new ArticleResult
                {
                    Success = false,
                    Message = "Статья не найдена"
                };
            }

            try
            {
                if (request.Approve)
                {
                    // Публикуем статью
                    article.Publish();

                    // Создаем уведомление через фабрику
                    var notificationFactory = new ArticlePublishedNotificationFactory(
                        article.AuthorId,
                        article.Id,
                        article.Title
                    );
                    var notification = notificationFactory.CreateNotification();
                    await _notificationRepository.AddNotificationAsync(notification);

                    await _repository.UpdateArticleStatus(article.Id, article.IsPublished);

                    return new ArticleResult
                    {
                        Success = true,
                        ArticleId = article.Id,
                        IsPublished = article.IsPublished,
                        Message = "Статья успешно опубликована"
                    };
                }
                else
                {
                    // Отклоняем статью
                    article.Reject(request.RejectionReason ?? "Причина не указана");
                    await _repository.UpdateArticle(article.Content, article.Id);

                    return new ArticleResult
                    {
                        Success = true,
                        Message = $"Статья отклонена. Причина: {request.RejectionReason ?? "Не указана"}"
                    };
                }
            }
            catch (InvalidOperationException ex)
            {
                return new ArticleResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}