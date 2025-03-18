using Application.Abstractions;
using Application.Articles.Command;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.CommandHandler
{
    public class CloneArticleHandler : IRequestHandler<CloneArticle, ArticleResult>
    {
        private readonly IArticleRepository _repository;

        public CloneArticleHandler(IArticleRepository repository)
        {
            _repository = repository;
        }

        public async Task<ArticleResult> Handle(CloneArticle request, CancellationToken cancellationToken)
        {
            var sourceArticle = await _repository.GetArticleById(request.SourceArticleId);
            if (sourceArticle == null)
            {
                return new ArticleResult { Success = false, Message = "Исходная статья не найдена" };
            }

            var clonedArticle = sourceArticle.Clone();
            if (!request.AsDraft)
            {
                clonedArticle.IsPublished = true;
            }

            var addedArticle = await _repository.AddArticle(clonedArticle);

            return new ArticleResult
            {
                Success = true,
                ArticleId = addedArticle.Id,
                IsPublished = addedArticle.IsPublished,
                Message = addedArticle.IsPublished
                    ? "Клонированная статья опубликована"
                    : "Клонированная статья сохранена как черновик"
            };
        }
    }
}
