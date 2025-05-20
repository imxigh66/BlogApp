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
    public class BlockArticleHandler : IRequestHandler<BlockArticleCommand, ArticleResult>
    {
        private readonly IArticleRepository _repository;

        public BlockArticleHandler(IArticleRepository repository)
        {
            _repository = repository;
        }

        public async Task<ArticleResult> Handle(BlockArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _repository.GetArticleById(request.ArticleId);
            if (article == null)
            {
                return new ArticleResult { Success = false, Message = "Статья не найдена" };
            }

            try
            {
                article.Block(request.Reason);
                await _repository.UpdateArticleWithState(article);

                return new ArticleResult
                {
                    Success = true,
                    ArticleId = article.Id,
                    IsPublished = article.IsPublished,
                    Message = $"Статья заблокирована. Причина: {request.Reason}"
                };
            }
            catch (InvalidOperationException ex)
            {
                return new ArticleResult { Success = false, Message = ex.Message };
            }
        }
    }
}
