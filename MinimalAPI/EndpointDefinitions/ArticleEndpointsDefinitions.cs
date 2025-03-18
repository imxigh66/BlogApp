using Application.Articles.Command;
using Application.Articles.Commands;
using Application.Articles.Queries;
using Application.Posts.Commands;
using Application.Posts.Queries;
using Domain.Enumerations;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MinimalAPI.Abstractions;
using MinimalAPI.Filters;
using System.Security.Claims;

namespace MinimalAPI.EndpointDefinitions
{
    public class ArticleEndpointsDefinitions : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            // Создание статьи (только для авторов)
            app.MapPost("api/articles", async (
                IMediator mediator,
                AddArticle command,
                HttpContext context) =>
            {
               
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userRating = int.Parse(context.User.FindFirstValue("Rating") ?? "0");
                var userRole = context.User.FindFirstValue(ClaimTypes.Role);

                bool needsModeration = userRole == UserRole.Author.ToString() && userRating < 50;

                command.AuthorId = userId;
                command.NeedsModeration = needsModeration;

                return await mediator.Send(command);
            }).RequireAuthorization("Author");

            
            app.MapGet("api/articles", async (IMediator mediator) =>
            {
                return await mediator.Send(new GetAllArticles());
            });


            // Получение статей на модерации (только для админов)
            app.MapGet("api/articles/pending", async (IMediator mediator) =>
            {
                return await mediator.Send(new GetPendingArticlesQuery());
            }).RequireAuthorization("Admin");



            app.MapGet("api/articles/{id}", async (int id, IMediator mediator) =>
            {
                return await mediator.Send(new GetArticlesById { ArticleId = id });
            });

            
            // Модерация статьи (только для админов)
            app.MapPost("api/articles/{id}/moderate", async (
                int id,
                ModerateArticleCommand command,
                IMediator mediator) =>
            {
                command.ArticleId = id;
                return await mediator.Send(command);
            }).RequireAuthorization("Admin");


            app.MapPost("api/articles/{id}/clone", async (
    int id,
    IMediator mediator,
    HttpContext context,
    [FromQuery] bool asDraft = true) =>
            {
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier));

                // Только автор статьи или администратор может клонировать статью
                var article = await mediator.Send(new GetArticlesById { ArticleId = id });
                if (article == null)
                {
                    return Results.NotFound();
                }

                var command = new CloneArticle
                {
                    SourceArticleId = id,
                    AsDraft = asDraft
                };

                var result = await mediator.Send(command);

                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            }).RequireAuthorization("Author");
        }

    }
}
