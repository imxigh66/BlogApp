using Application.Content;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Abstractions;

namespace MinimalAPI.EndpointDefinitions
{
    public class ContentEndpointDefinitions : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            // Эндпоинт для создания контента
            app.MapPost("api/content", ([FromBody] CreateContentRequest request) =>
            {
                var contentService = new ContentService();
                var content = contentService.CreateContent(
                    request.Type,
                    request.Title,
                    request.Parameters
                );

                return Results.Ok(new
                {
                    title = content.Title,
                    rendered = content.Render(),
                    createdAt = content.CreatedAt
                });
            });

            // Эндпоинт для создания текстового контента (упрощённый)
            app.MapPost("api/content/text", ([FromBody] CreateTextContentRequest request) =>
            {
                var contentService = new ContentService();
                var parameters = new Dictionary<string, string> { ["body"] = request.Body };
                var content = contentService.CreateContent(ContentType.Text, request.Title, parameters);

                return Results.Ok(new
                {
                    title = content.Title,
                    rendered = content.Render(),
                    createdAt = content.CreatedAt
                });
            });

            // Эндпоинт для создания контента с изображением (упрощённый)
            app.MapPost("api/content/image", ([FromBody] CreateImageContentRequest request) =>
            {
                var contentService = new ContentService();
                var parameters = new Dictionary<string, string>
                {
                    ["url"] = request.ImageUrl,
                    ["alt"] = request.AltText
                };
                var content = contentService.CreateContent(ContentType.Image, request.Title, parameters);

                return Results.Ok(new
                {
                    title = content.Title,
                    rendered = content.Render(),
                    createdAt = content.CreatedAt
                });
            });
        }

        public class CreateContentRequest
        {
            public ContentType Type { get; set; }
            public string Title { get; set; }
            public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
        }

        public class CreateTextContentRequest
        {
            public string Title { get; set; }
            public string Body { get; set; }
        }

        public class CreateImageContentRequest
        {
            public string Title { get; set; }
            public string ImageUrl { get; set; }
            public string AltText { get; set; }
        }
    }
}

