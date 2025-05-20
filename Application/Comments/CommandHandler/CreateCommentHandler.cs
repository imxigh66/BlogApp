// Application/Comments/CommandHandler/CreateCommentHandler.cs
using Application.Abstractions;
using Application.Comments.Command;
using Application.Comments;
using Domain.Handlers;
using MediatR;
using Application.Handlers;

namespace Application.Comments.CommandHandler
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CommentResult>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationSender _notificationSender;

        public CreateCommentHandler(
            ICommentRepository commentRepository,
            IArticleRepository articleRepository,
            INotificationRepository notificationRepository,
            INotificationSender notificationSender)
        {
            _commentRepository = commentRepository;
            _articleRepository = articleRepository;
            _notificationRepository = notificationRepository;
            _notificationSender = notificationSender;
        }

        public async Task<CommentResult> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetArticleById(request.ArticleId);
            if (article == null || !article.IsPublished)
            {
                return new CommentResult { Success = false, Message = "Статья не найдена или не опубликована" };
            }

            if (!article.CanComment())
            {
                return new CommentResult { Success = false, Message = "Комментирование в текущем состоянии статьи запрещено" };
            }

            try
            {
                // Создаем фабрику обработчиков комментариев
                var handlerFactory = new CommentHandlerFactory(_commentRepository);

                // Создаем цепочку обработчиков
                var commentHandler = handlerFactory.CreateCommentProcessingChain(
                    request.ArticleId,
                    string.IsNullOrWhiteSpace(request.AnonymousName) ? "Аноним" : request.AnonymousName);

                // Запускаем обработку через цепочку
                string processedComment = commentHandler.Handle(request.Content);

                // Если мы дошли до этой точки, значит, комментарий был успешно сохранен
                // Находим последний добавленный комментарий для этой статьи
                var comments = await _commentRepository.GetByArticleIdAsync(request.ArticleId);
                var addedComment = comments.OrderByDescending(c => c.PostedAt).FirstOrDefault();

                if (addedComment == null)
                {
                    return new CommentResult { Success = false, Message = "Ошибка при добавлении комментария" };
                }

                // Отправляем уведомление автору статьи
                var notificationFactory = new Notifications.NewCommentNotificationFactory(
                    article.AuthorId,
                    article.Id,
                    addedComment.Id,
                    addedComment.Username);

                var notification = notificationFactory.CreateNotification();
                await _notificationSender.SendAsync(notification);

                return new CommentResult
                {
                    Success = true,
                    CommentId = addedComment.Id,
                    Message = "Комментарий добавлен"
                };
            }
            catch (InvalidOperationException ex)
            {
                return new CommentResult { Success = false, Message = ex.Message };
            }
            catch (Exception)
            {
                return new CommentResult { Success = false, Message = "Произошла ошибка при обработке комментария" };
            }
        }
    }
}