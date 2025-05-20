using Application.Abstractions;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public class CommentSaver : BaseCommentHandler
    {
        private readonly ICommentRepository _repository;
        private readonly int _articleId;
        private readonly string _username;

        public CommentSaver(ICommentRepository repository, int articleId, string username)
        {
            _repository = repository;
            _articleId = articleId;
            _username = username;
        }

        public override string Handle(string comment)
        {
            if (string.IsNullOrEmpty(comment))
                throw new InvalidOperationException("Нельзя сохранить пустой комментарий");

            // Создаем новый комментарий
            var newComment = new Comment
            {
                ArticleId = _articleId,
                Username = _username,
                Content = comment,
                PostedAt = DateTime.UtcNow
            };

            // Сохраняем в репозиторий
            _repository.AddAsync(newComment).Wait();

            // Возвращаем финальную версию комментария
            return base.Handle(comment);
        }
    }
}
