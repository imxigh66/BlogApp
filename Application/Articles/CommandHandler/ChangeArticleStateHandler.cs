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
    public class ChangeArticleStateHandler : IRequestHandler<ChangeArticleStateCommand, ArticleResult>
    {
        private readonly IArticleRepository _repository;

        public ChangeArticleStateHandler(IArticleRepository repository)
        {
            _repository = repository;
        }

        public async Task<ArticleResult> Handle(ChangeArticleStateCommand request, CancellationToken cancellationToken)
        {
            var article = await _repository.GetArticleById(request.ArticleId);
            if (article == null)
            {
                return new ArticleResult { Success = false, Message = "Статья не найдена" };
            }

            try
            {
                switch (request.Action)
                {
                    case "SendToModeration":
                        article.SendToModeration();
                        break;
                    case "Draft":
                        article.Draft();
                        break;
                    default:
                        return new ArticleResult { Success = false, Message = "Неизвестное действие" };
                }

                await _repository.UpdateArticleWithState(article);

                return new ArticleResult
                {
                    Success = true,
                    ArticleId = article.Id,
                    IsPublished = article.IsPublished,
                    Message = $"Статья переведена в состояние '{article.StateName}'"
                };
            }
            catch (InvalidOperationException ex)
            {
                return new ArticleResult { Success = false, Message = ex.Message };
            }
        }
    }
}
