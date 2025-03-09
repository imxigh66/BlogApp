using MediatR;
using Domain.Models;

using Application.Articles.Commands;
using Application.Abstractions;

namespace Application.Articles.CommandHandlers
{
    public class AddArticleHandler : IRequestHandler<AddArticle, ArticleResult>
    {
        private readonly IArticleRepository _repository;
        private readonly IAuthRepository _authRepository; // Для получения автора

        public AddArticleHandler(IArticleRepository repository, IAuthRepository authRepository)
        {
            _repository = repository;
            _authRepository = authRepository;
        }

        public async Task<ArticleResult> Handle(AddArticle request, CancellationToken cancellationToken)
        {
            // Получаем автора через репозиторий авторов
            var author = await _authRepository.GetUserByIdAsync(request.AuthorId);
            if (author == null)
            {
                return new ArticleResult { Success = false, Message = "Автор не найден" };
            }

            var article = new Article
            {
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                AuthorId = request.AuthorId,
                IsPublished = !request.NeedsModeration
            };

            // Используем репозиторий для добавления статьи
            var addedArticle = await _repository.AddArticle(article);

            return new ArticleResult
            {
                Success = true,
                ArticleId = addedArticle.Id,
                IsPublished = addedArticle.IsPublished,
                Message = addedArticle.IsPublished
                    ? "Статья опубликована"
                    : "Статья отправлена на модерацию"
            };
        }
    }
}
