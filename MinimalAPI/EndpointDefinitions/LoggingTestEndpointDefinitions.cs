using Application.Abstractions;
using Domain.Models;
using MinimalAPI.Abstractions;

namespace MinimalAPI.EndpointDefinitions
{
    public class LoggingTestEndpointDefinitions : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("api/test/logging", async (
                IArticleRepository articleRepository,
                Application.Abstractions.ILogger logger) =>
            {
                try
                {
                    // Тестирование логирования в репозитории статей
                    logger.LogInfo("=== Начало теста логирования ===");

                    // 1. Получение всех статей
                    var allArticles = await articleRepository.GetAllArticle();

                    // 2. Если есть статьи, тестируем получение статьи по ID
                    Article article = null;
                    if (allArticles.Count > 0)
                    {
                        article = await articleRepository.GetArticleById(allArticles.First().Id);
                    }

                    // 3. Создание новой тестовой статьи для логирования
                    var testArticle = new Article
                    {
                        Title = $"Тестовая статья для логирования {DateTime.Now}",
                        Content = "Это тестовая статья для проверки логирования",
                        AuthorId = 1, // Убедитесь, что такой пользователь существует
                        IsPublished = false
                    };

                    var createdArticle = await articleRepository.AddArticle(testArticle);

                    // 4. Обновление статьи
                    await articleRepository.UpdateArticle("Обновленное содержимое тестовой статьи", createdArticle.Id);

                    // 5. Изменение статуса статьи
                    await articleRepository.UpdateArticleStatus(createdArticle.Id, true);

                    // 6. Получение статей на модерации
                    await articleRepository.GetPendingArticles();

                    // 7. Удаление тестовой статьи
                    await articleRepository.DeleteArticle(createdArticle.Id);

                    // 8. Попытка получить несуществующую статью (для тестирования предупреждения)
                    await articleRepository.GetArticleById(999999);

                    logger.LogInfo("=== Конец теста логирования ===");

                    // Чтение файла логов для отображения результата
                    string logContent = "Логи недоступны";
                    try
                    {
                        logContent = File.ReadAllText("logs.txt");
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest($"Не удалось прочитать файл логов: {ex.Message}");
                    }

                    return Results.Ok(new
                    {
                        message = "Тест логирования выполнен успешно",
                        log = logContent
                    });
                }
                catch (Exception ex)
                {
                    logger.LogError("Ошибка при выполнении теста логирования", ex);
                    return Results.BadRequest($"Ошибка при выполнении теста логирования: {ex.Message}");
                }
            });
        }
    }
}
