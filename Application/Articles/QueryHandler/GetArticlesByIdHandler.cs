using Application.Abstractions;
using Application.Articles.Queries;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.QueryHandler
{
    public class GetArticlesByIdHandler : IRequestHandler<GetArticlesById, Article>
    {
        private readonly IArticleRepository _repository;
        public GetArticlesByIdHandler(IArticleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Article> Handle(GetArticlesById request, CancellationToken cancellationToken)
        {
            return await _repository.GetArticleById(request.ArticleId);
        }
    }
}
