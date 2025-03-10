using Application.Likes.Command;
using Application.Likes.Query;
using MediatR;
using MinimalAPI.Abstractions;

namespace MinimalAPI.EndpointDefinitions
{
    public class LikeEndpointDefinitions : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            // Добавление/удаление лайка
            app.MapPost("api/articles/{articleId}/like", async (
                int articleId,
                IMediator mediator,
                HttpContext context) =>
            {
                var command = new ToggleLikeCommand
                {
                    ArticleId = articleId,
                    IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown"
                };
                return await mediator.Send(command);
            });

            // Получение количества лайков статьи
            app.MapGet("api/articles/{articleId}/likes/count", async (
                int articleId,
                IMediator mediator) =>
            {
                return await mediator.Send(new GetArticleLikesCountQuery { ArticleId = articleId });
            });
        }
    }
}
