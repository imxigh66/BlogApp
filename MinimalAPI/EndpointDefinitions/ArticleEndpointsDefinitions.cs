using Application.Articles.Commands;
using Application.Articles.Queries;
using Application.Posts.Commands;
using Application.Posts.Queries;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Hosting;
using MinimalAPI.Abstractions;
using MinimalAPI.Filters;

namespace MinimalAPI.EndpointDefinitions
{
    public class ArticleEndpointsDefinitions : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            var articles = app.MapGroup("/api/article");

            articles.MapGet("/{id}", GetArticleById)
                .WithName("GetArticleById");

            articles.MapPost("/", AddArtcile);

            articles.MapGet("/", GetAllArticles);

            articles.MapPut("/{id}", UpdateArticle);

            articles.MapDelete("/{id}", DeleteArticle);
        }

        private async Task<IResult> GetArticleById(IMediator mediator, int id)
        {
            var getArticle = new GetArticlesById { ArticleId = id };
            var article = await mediator.Send(getArticle);
            return TypedResults.Ok(article);
        }

        private async Task<IResult> AddArtcile(IMediator mediator, Article article)
        {
            var addArticle = new AddArticle { Content = article.Content };
            var addedArticle = await mediator.Send(addArticle);
            return Results.CreatedAtRoute("GetArticleById", new { addedArticle.Id }, addedArticle);
        }

        private async Task<IResult> GetAllArticles(IMediator mediator)
        {
            var getCommand = new GetAllArticles();
            var articles = await mediator.Send(getCommand);
            return TypedResults.Ok(articles);
        }

        private async Task<IResult> UpdateArticle(IMediator mediator, Article article, int id)
        {
            var updateArticle = new UpdateArticle { ArticleId = id, Content = article.Content };
            var updatedArticle = await mediator.Send(updateArticle);
            return TypedResults.Ok(updatedArticle);
        }

        private async Task<IResult> DeleteArticle(IMediator mediator, int id)
        {
            var deleteArticle = new DeleteArticle { ArticleId = id };
            await mediator.Send(deleteArticle);
            return TypedResults.NoContent();
        }

    }
}
