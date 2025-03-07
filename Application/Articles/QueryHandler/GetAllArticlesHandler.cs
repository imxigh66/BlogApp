using Application.Abstractions;
using Application.Articles.Queries;
using Application.Posts.Queries;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.QueryHandler
{
    public class GetAllArticlesHandler : IRequestHandler<GetAllArticles, ICollection<Article>>
    {
        private readonly IArticleRepository _repository;
        public GetAllArticlesHandler(IArticleRepository repository)
        {
            _repository = repository;
        }
      
        public async Task<ICollection<Article>> Handle(GetAllArticles request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllArticle();
        }
    }
}
