using DataAccess;
using MinimalAPI.Abstractions;

namespace MinimalAPI.EndpointDefinitions
{
    public class ImageEndpointDefinitions : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            // Эндпоинт для получения изображений из БД
            app.MapGet("api/images/{id}", async (
                string id,
                BlogDbContext dbContext) =>
            {
                var image = await dbContext.Images.FindAsync(id);
                if (image == null)
                    return Results.NotFound();

                return Results.File(image.Data, image.ContentType);
            });
        }
    }
}
