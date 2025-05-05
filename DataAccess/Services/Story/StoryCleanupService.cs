using Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.Story
{
    public class StoryCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<StoryCleanupService> _logger;

        public StoryCleanupService(
            IServiceProvider serviceProvider,
            ILogger<StoryCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Запуск процесса очистки истекших историй");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var storyRepository = scope.ServiceProvider.GetRequiredService<IStoryRepository>();
                        var deletedCount = await storyRepository.CleanExpiredStoriesAsync();

                        _logger.LogInformation($"Удалено {deletedCount} истекших историй");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка во время очистки историй");
                }

                // Запускаем очистку каждый час
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
