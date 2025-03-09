using Application.Abstractions;
using Application.Articles.Dto;
using Application.Articles.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.QueryHandler
{
    public class GetPendingArticlesHandler : IRequestHandler<GetPendingArticlesQuery, List<ArticleDto>>
    {
        private readonly IArticleRepository _repository;
        public GetPendingArticlesHandler(IArticleRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<ArticleDto>> Handle(GetPendingArticlesQuery request, CancellationToken cancellationToken)
        {
            var pendingArticles = await _repository.GetPendingArticles();

            return pendingArticles.Select(a => new ArticleDto
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                CreatedAt = a.CreatedAt,
                AuthorName = a.Author.Username,
                AuthorId = a.AuthorId,
                AuthorRating = a.Author.Rating
            }).ToList();
        }
    }
}
