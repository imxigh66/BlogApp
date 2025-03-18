using Application.Abstractions;
using Application.Articles.Builder;
using Application.Articles.Command;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.CommandHandler
{
    public class AddRichArticleHandler : IRequestHandler<AddRichArticle, ArticleResult>
    {
        private readonly IArticleRepository _repository;
        private readonly IAuthRepository _authRepository;

        public AddRichArticleHandler(IArticleRepository repository, IAuthRepository authRepository)
        {
            _repository = repository;
            _authRepository = authRepository;
        }

        public async Task<ArticleResult> Handle(AddRichArticle request, CancellationToken cancellationToken)
        {
            var author = await _authRepository.GetUserByIdAsync(request.AuthorId);
            if (author == null)
            {
                return new ArticleResult { Success = false, Message = "Автор не найден" };
            }

            // Используем строителя для создания статьи
            var builder = new ArticleBuilder();
            var director = new ArticleDirector(builder);

            // Создаем список контента для директора
            var contentItems = request.ContentItems
                .Select(item => (item.Type, item.Title, item.Parameters))
                .ToList();

            // Создаем статью через директора
            var article = director.CreateRichMediaArticle(
                request.Title,
                request.Content,
                request.AuthorId,
                contentItems,
                !request.NeedsModeration
            );

            // Сохраняем статью в базу данных
            var addedArticle = await _repository.AddArticle(article);

            return new ArticleResult
            {
                Success = true,
                ArticleId = addedArticle.Id,
                IsPublished = addedArticle.IsPublished,
                Message = addedArticle.IsPublished
                    ? "Статья с мультимедиа опубликована"
                    : "Статья с мультимедиа отправлена на модерацию"
            };
        }
    }
}
