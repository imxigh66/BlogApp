using Application.Abstractions;
using Application.Comments.Command;
using Application.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comments.CommandHandler
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CommentResult>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly INotificationRepository _notificationRepository;

        private readonly INotificationSender _notificationSender;

        public CreateCommentHandler(ICommentRepository commentRepository, IArticleRepository articleRepository, INotificationRepository notificationRepository, INotificationSender notificationSender)
        {
            _commentRepository = commentRepository;
            _articleRepository = articleRepository;
            _notificationRepository = notificationRepository;
            _notificationSender = notificationSender;
        }
        public async Task<CommentResult> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var article = await  _articleRepository.GetArticleById(request.ArticleId);
            if(article==null || !article.IsPublished)
            {
                return new CommentResult { Success = false, Message = "Статья не найдена или не опубликована" };
            }
            var comment = new Domain.Models.Comment
            {
                Content = request.Content,
                PostedAt = DateTime.UtcNow,
                Username = string.IsNullOrWhiteSpace(request.AnonymousName) ? "Аноним" : request.AnonymousName,
                ArticleId = request.ArticleId
            };

            var addedComment = await _commentRepository.AddAsync(comment);

            // Создаем уведомление для автора статьи с использованием фабричного метода
            var notificationFactory = new NewCommentNotificationFactory(
                article.AuthorId,
                article.Id,
                addedComment.Id,
                comment.Username);

            var notification = notificationFactory.CreateNotification();
            //await _notificationRepository.AddNotificationAsync(notification);

            await _notificationSender.SendAsync(notification);

            return new CommentResult
            {
                Success = true,
                CommentId = addedComment.Id,
                Message = "Комментарий добавлен"
            };

        }
    }
}
