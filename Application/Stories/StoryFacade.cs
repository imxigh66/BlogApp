using Application.Abstractions;
using Application.Stories.Dto;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Stories
{
    public class StoryFacade
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IStoryMediaService _mediaService;
        private readonly IStoryNotificationService _notificationService;
        private readonly ILogger<StoryFacade> _logger;

        public StoryFacade(
            IStoryRepository storyRepository,
            IStoryMediaService mediaService,
            IStoryNotificationService notificationService,
            ILogger<StoryFacade> logger)
        {
            _storyRepository = storyRepository;
            _mediaService = mediaService;
            _notificationService = notificationService;
            _logger = logger;
        }

        // Создание новой истории
        public async Task<StoryDto> CreateStoryAsync(CreateStoryDto createDto, int authorId)
        {
            try
            {
                // 1. Обработка медиа-файла (если есть)
                string mediaUrl = null;
                if (createDto.Media != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await createDto.Media.CopyToAsync(memoryStream);
                        mediaUrl = await _mediaService.UploadMediaAsync(
                            memoryStream.ToArray(),
                            createDto.Media.FileName
                        );
                    }
                }

                // 2. Создание и сохранение истории
                var story = new Story
                {
                    Content = createDto.Content,
                    MediaUrl = mediaUrl,
                    Type = createDto.Type,
                    AuthorId = authorId,
                    CreatedAt = DateTime.UtcNow
                };

                var createdStory = await _storyRepository.CreateStoryAsync(story);

                // 3. Отправка уведомлений подписчикам
                await _notificationService.NotifyNewStoryAsync(createdStory);

                // 4. Возврат DTO
                return new StoryDto
                {
                    Id = createdStory.Id,
                    Content = createdStory.Content,
                    MediaUrl = createdStory.MediaUrl,
                    Type = createdStory.Type.ToString(),
                    AuthorId = createdStory.AuthorId,
                    AuthorName = createdStory.Author?.Username ?? "Unknown",
                    CreatedAt = createdStory.CreatedAt,
                    ExpiresAt = createdStory.ExpiresAt,
                    ViewsCount = 0,
                    HasBeenViewedByCurrentUser = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании истории");
                throw;
            }
        }

        // Получение ленты историй
        public async Task<StoryFeedDto> GetStoryFeedAsync(int viewerId)
        {
            try
            {
                // 1. Получаем активные истории
                var stories = await _storyRepository.GetActiveStoriesForFeedAsync(viewerId);

                // 2. Группируем истории по авторам
                var feed = new StoryFeedDto();
                var authorGroups = stories
                    .GroupBy(s => s.AuthorId)
                    .Select(g => new AuthorStoriesGroupDto
                    {
                        AuthorId = g.Key,
                        AuthorName = g.First().Author?.Username ?? "Unknown",
                        Stories = g.Select(s => new StoryDto
                        {
                            Id = s.Id,
                            Content = s.Content,
                            MediaUrl = s.MediaUrl,
                            Type = s.Type.ToString(),
                            AuthorId = s.AuthorId,
                            AuthorName = s.Author?.Username ?? "Unknown",
                            CreatedAt = s.CreatedAt,
                            ExpiresAt = s.ExpiresAt,
                            ViewsCount = s.Views.Count,
                            HasBeenViewedByCurrentUser = s.Views.Any(v => v.ViewerId == viewerId)
                        }).ToList()
                    })
                    .ToList();

                feed.AuthorGroups = authorGroups;
                return feed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении ленты историй");
                throw;
            }
        }

        // Просмотр истории
        public async Task<StoryDto> ViewStoryAsync(int storyId, int viewerId)
        {
            try
            {
                // 1. Получаем историю
                var story = await _storyRepository.GetStoryByIdAsync(storyId);
                if (story == null || !story.IsActive)
                {
                    throw new Exception("История не найдена или истекла");
                }

                // 2. Записываем просмотр
                var storyView = await _storyRepository.AddStoryViewAsync(storyId, viewerId);

                // 3. Отправляем уведомление автору
                await _notificationService.NotifyStoryViewedAsync(storyView);

                // 4. Возвращаем DTO
                return new StoryDto
                {
                    Id = story.Id,
                    Content = story.Content,
                    MediaUrl = story.MediaUrl,
                    Type = story.Type.ToString(),
                    AuthorId = story.AuthorId,
                    AuthorName = story.Author?.Username ?? "Unknown",
                    CreatedAt = story.CreatedAt,
                    ExpiresAt = story.ExpiresAt,
                    ViewsCount = story.Views.Count,
                    HasBeenViewedByCurrentUser = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при просмотре истории {storyId}");
                throw;
            }
        }

        // Удаление истории
        public async Task<bool> DeleteStoryAsync(int storyId, int authorId)
        {
            try
            {
                // 1. Получаем историю
                var story = await _storyRepository.GetStoryByIdAsync(storyId);
                if (story == null)
                {
                    return false;
                }

                // 2. Проверяем права (только автор может удалить)
                if (story.AuthorId != authorId)
                {
                    return false;
                }

                // 3. Удаляем медиа-файл
                if (!string.IsNullOrEmpty(story.MediaUrl))
                {
                    await _mediaService.DeleteMediaAsync(story.MediaUrl);
                }

                // 4. Удаляем историю из базы
                return await _storyRepository.DeleteStoryAsync(storyId, authorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении истории {storyId}");
                return false;
            }
        }

        // Получение историй пользователя
        public async Task<List<StoryDto>> GetUserStoriesAsync(int userId, int viewerId)
        {
            try
            {
                var stories = await _storyRepository.GetActiveStoriesByUserAsync(userId);

                return stories.Select(s => new StoryDto
                {
                    Id = s.Id,
                    Content = s.Content,
                    MediaUrl = s.MediaUrl,
                    Type = s.Type.ToString(),
                    AuthorId = s.AuthorId,
                    AuthorName = s.Author?.Username ?? "Unknown",
                    CreatedAt = s.CreatedAt,
                    ExpiresAt = s.ExpiresAt,
                    ViewsCount = s.Views.Count,
                    HasBeenViewedByCurrentUser = s.Views.Any(v => v.ViewerId == viewerId)
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении историй пользователя {userId}");
                throw;
            }
        }
    }
}
