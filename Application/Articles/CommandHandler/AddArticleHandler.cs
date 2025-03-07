using MediatR;
using Domain.Models;

using Application.Articles.Commands;
using Application.Abstractions;

namespace Application.Articles.CommandHandlers
{
    public class AddArticleHandler : IRequestHandler<AddArticle, Article>
    {
        private readonly IArticleRepository _repository;

        public AddArticleHandler(IArticleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Article> Handle(AddArticle request, CancellationToken cancellationToken)
        {
            var article = new Article
            {
                Title = request.Title,
                Content = request.Content,
                AuthorId = request.AuthorId,
                CreatedAt = DateTime.UtcNow
            };

            return await _repository.AddArticle(article);
        }
    }
}
