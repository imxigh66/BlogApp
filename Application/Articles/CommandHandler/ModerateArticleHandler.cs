﻿using Application.Abstractions;
using Application.Articles.Command;
using Application.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (article.IsPublished)
            {
                return new ArticleResult
                {
                    Success = false,
                    Message = "Статья уже опубликована"
                };
            }
            if (request.Approve)
            {
                // Публикуем статью
                article.IsPublished = true;
                await _repository.UpdateArticleStatus(article.Id, true);

                // Создаем уведомление через фабрику
                var notificationFactory = new ArticlePublishedNotificationFactory(
                    article.AuthorId,
                    article.Id,
                    article.Title
                );
                var notification = notificationFactory.CreateNotification();
                await _notificationRepository.AddNotificationAsync(notification);

                return new ArticleResult
                {
                    Success = true,
                    ArticleId = article.Id,
                    IsPublished = true,
                    Message = "Статья успешно опубликована"
                };
            }
            else
            {
                // Отклоняем статью
                await _repository.DeleteArticle(article.Id);

                return new ArticleResult
                {
                    Success = true,
                    Message = $"Статья отклонена. Причина: {request.RejectionReason ?? "Не указана"}"
                };
            }
        }
    }
}
