using Application.Abstractions;
using Application.Articles;
using Application.Articles.Builder;
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


            app.MapPost("api/articles/rich", async (
    IMediator mediator,
    AddRichArticle command,
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



            app.MapPost("api/articles/with-image", async (
    HttpContext context,
    IFormFile image,
    [FromForm] string title,
    [FromForm] string content,
    IMediator mediator,
    IImageManager imageManager) =>
            {
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userRole = context.User.FindFirstValue(ClaimTypes.Role);
                bool needsModeration = userRole == UserRole.Author.ToString() &&
                                       int.Parse(context.User.FindFirstValue("Rating") ?? "0") < 50;

                // Чтение изображения в память
                using var stream = new MemoryStream();
                await image.CopyToAsync(stream);
                var imageBytes = stream.ToArray();

                // Сохранение изображения через IImageManager
                string imageUrl = await imageManager.StoreImageAsync(
                    imageBytes,
                    image.FileName,
                    title); // Используем заголовок как alt-текст

                // Создание статьи с изображением
                var articleBuilder = new ArticleBuilder(imageManager);
                var article = articleBuilder
                    .SetTitle(title)
                    .SetContent(content)
                    .SetAuthor(userId)
                    .SetIsPublished(!needsModeration)
                    .AddImageContent("Featured Image", imageUrl, title)
                    .Build();

                // Сохранение статьи в базу данных
                var articleRepository = context.RequestServices.GetRequiredService<IArticleRepository>();
                var addedArticle = await articleRepository.AddArticle(article);

                return Results.Ok(new ArticleResult
                {
                    Success = true,
                    ArticleId = addedArticle.Id,
                    IsPublished = addedArticle.IsPublished,
                    Message = addedArticle.IsPublished
                        ? "Статья с изображением опубликована"
                        : "Статья с изображением отправлена на модерацию"
                });
            }).RequireAuthorization("Author")
            .DisableAntiforgery();

            app.MapPost("api/articles/{id}/send-to-moderation", async (
    int id,
    IArticleRepository repository,
    HttpContext context) =>
            {
                var article = await repository.GetArticleById(id);
                if (article == null)
                    return Results.NotFound();

                try
                {
                    article.SendToModeration();
                    await repository.UpdateArticleWithState(article);

                    return Results.Ok(new
                    {
                        success = true,
                        articleId = article.Id,
                        state = article.StateName,
                        message = $"Статья переведена в состояние '{article.StateName}'"
                    });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
            }).RequireAuthorization("Author");

            // Блокировка статьи
            app.MapPost("api/articles/{id}/block", async (
                int id,
                BlockArticleCommand command,
                IMediator mediator) =>
            {
                command.ArticleId = id;
                try
                {
                    var result = await mediator.Send(command);
                    return Results.Ok(result);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
            }).RequireAuthorization("Admin");

            // Получение информации о состоянии статьи
            app.MapGet("api/articles/{id}/state", async (
                int id,
                IArticleRepository repository) =>
            {
                var article = await repository.GetArticleById(id);
                if (article == null)
                    return Results.NotFound();

                return Results.Ok(new
                {
                    id = article.Id,
                    title = article.Title,
                    state = article.StateName,
                    reason = article.StateReason,
                    canLike = article.CanLike(),
                    canComment = article.CanComment(),
                    canEdit = article.CanEdit()
                });
            });
        }



    }
}
