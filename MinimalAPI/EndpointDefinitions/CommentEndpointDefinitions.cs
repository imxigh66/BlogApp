using Application.Comments.Command;
using Application.Comments.Query;
using MediatR;
using MinimalAPI.Abstractions;

namespace MinimalAPI.EndpointDefinitions
{
    public class CommentEndpointDefinitions : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            // Добавление комментария
            app.MapPost("api/articles/{articleId}/comments", async (
                int articleId,
                CreateCommentCommand command,
                IMediator mediator) =>
            {
                command.ArticleId = articleId;
                return await mediator.Send(command);
            });

            // Получение комментариев к статье
            app.MapGet("api/articles/{articleId}/comments", async (
                int articleId,
                IMediator mediator) =>
            {
                return await mediator.Send(new GetArticleCommentsQuery { ArticleId = articleId });
            });
        }
    }
}
