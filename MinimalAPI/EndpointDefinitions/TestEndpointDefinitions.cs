using Application.Abstractions;
using DataAccess.Services;
using MinimalAPI.Abstractions;

namespace MinimalAPI.EndpointDefinitions
{
    public class TestEndpointDefinitions : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("api/test/cache-stats", () =>
            {
                return CacheService.Instance.GetStats();
            });
            //.RequireAuthorization("Admin"); // Только для админов

            app.MapGet("api/test/cache-test", async (IArticleRepository articleRepository) =>
            {
                // Первый запрос (должен идти в БД и кэшироваться)
                var startTime1 = DateTime.UtcNow;
                var article1 = await articleRepository.GetArticleById(1);
                var endTime1 = DateTime.UtcNow;
                var duration1 = (endTime1 - startTime1).TotalMilliseconds;

                // Второй запрос (должен идти из кэша)
                var startTime2 = DateTime.UtcNow;
                var article2 = await articleRepository.GetArticleById(1);
                var endTime2 = DateTime.UtcNow;
                var duration2 = (endTime2 - startTime2).TotalMilliseconds;

                return new
                {
                    FirstRequestMs = duration1,
                    SecondRequestMs = duration2,
                    FromCacheImprovement = duration1 > 0 ? $"{(1 - duration2 / duration1) * 100:F2}%" : "N/A",
                    // Не возвращаем объект article целиком, а только его основные свойства
                    ArticleInfo = article1 != null ? new
                    {
                        article1.Id,
                        article1.Title,
                        AuthorName = article1.Author?.Username,
                        article1.CreatedAt
                    } : null
                };
            });
        }
    }
}
