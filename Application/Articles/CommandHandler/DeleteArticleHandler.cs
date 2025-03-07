using Application.Abstractions;
using Application.Articles.Commands;
using Application.Posts.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.CommandHandler
{
    public class DeleteArticleHandler : IRequestHandler<DeleteArticle>
    {
        private readonly IArticleRepository _repository;

        public DeleteArticleHandler(IArticleRepository repository)
        {
            _repository = repository;
        }



        public async Task Handle(DeleteArticle request, CancellationToken cancellationToken)
        {
            await _repository.DeleteArticle(request.ArticleId);
        }
    }
}
