using Application.Stories;
using Application.Stories.Dto;
using Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Abstractions;
using System.Security.Claims;

namespace MinimalAPI.EndpointDefinitions
{
    public class StoryEndpointDefinitions : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            var stories = app.MapGroup("/api/stories");

            // Получение ленты историй
            stories.MapGet("/feed", async (
                HttpContext context,
                StoryFacade storyFacade) =>
            {
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                return await storyFacade.GetStoryFeedAsync(userId);
            }).RequireAuthorization();

            // Создание новой истории
            stories.MapPost("/", async (
                HttpContext context,
                IFormFile? file,
                [FromForm] string content,
                [FromForm] StoryType type,
                StoryFacade storyFacade) =>
            {
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

                var createDto = new CreateStoryDto
                {
                    Content = content,
                    Media = file,
                    Type = type
                };

                var story = await storyFacade.CreateStoryAsync(createDto, userId);
                return Results.Created($"/api/stories/{story.Id}", story);
            }).RequireAuthorization().DisableAntiforgery();

            // Просмотр истории
            stories.MapGet("/{id}/view", async (
                int id,
                HttpContext context,
                StoryFacade storyFacade) =>
            {
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                try
                {
                    var story = await storyFacade.ViewStoryAsync(id, userId);
                    return Results.Ok(story);
                }
                catch (Exception ex)
                {
                    return Results.NotFound(ex.Message);
                }
            }).RequireAuthorization();

            // Получение историй пользователя
            stories.MapGet("/user/{userId}", async (
                int userId,
                HttpContext context,
                StoryFacade storyFacade) =>
            {
                var viewerId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                var stories = await storyFacade.GetUserStoriesAsync(userId, viewerId);
                return Results.Ok(stories);
            }).RequireAuthorization();

            // Удаление истории
            stories.MapDelete("/{id}", async (
                int id,
                HttpContext context,
                StoryFacade storyFacade) =>
            {
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                var success = await storyFacade.DeleteStoryAsync(id, userId);

                return success ? Results.Ok() : Results.NotFound();
            }).RequireAuthorization();
        }
    }
}
