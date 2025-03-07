using Application.Abstractions;
using Application.Articles.Commands;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.CommandHandler
{
    public class UpdateArticleHandler : IRequestHandler<UpdateArticle, Article>
    {
        private readonly IArticleRepository _repository;

        public UpdateArticleHandler(IArticleRepository repository)
        {
            _repository = repository;
        }
        public async Task<Article> Handle(UpdateArticle request, CancellationToken cancellationToken)
        {
            var article = await _repository.UpdateArticle(request.Content, request.ArticleId);
            return article;
        }
    }
}
