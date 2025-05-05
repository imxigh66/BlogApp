using Application.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly BlogDbContext _context;

        public StoryRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<Story> CreateStoryAsync(Story story)
        {
            // Установка времени истечения (24 часа от создания)
            story.ExpiresAt = story.CreatedAt.AddHours(24);

            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();

            // Загружаем автора перед возвратом
            return await _context.Stories
                .Include(s => s.Author)
                .FirstOrDefaultAsync(s => s.Id == story.Id);
        }

        public async Task<Story> GetStoryByIdAsync(int id)
        {
            return await _context.Stories
                .Include(s => s.Author)
                .Include(s => s.Views)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Story>> GetActiveStoriesByUserAsync(int userId)
        {
            var now = DateTime.UtcNow;
            return await _context.Stories
                .Include(s => s.Author)
                .Include(s => s.Views)
                .Where(s => s.AuthorId == userId && s.ExpiresAt > now)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Story>> GetActiveStoriesForFeedAsync(int viewerId, int count = 20)
        {
            var now = DateTime.UtcNow;

            // В реальном приложении здесь может быть сложная логика выборки историй
            // от пользователей, на которых подписан текущий пользователь
            return await _context.Stories
                .Include(s => s.Author)
                .Include(s => s.Views)
                .Where(s => s.ExpiresAt > now)
                .OrderByDescending(s => s.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<StoryView> AddStoryViewAsync(int storyId, int viewerId)
        {
            // Проверяем, не просмотрел ли уже этот пользователь историю
            var existingView = await _context.StoryViews
                .FirstOrDefaultAsync(v => v.StoryId == storyId && v.ViewerId == viewerId);

            if (existingView != null)
            {
                // Обновляем время просмотра
                existingView.ViewedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return existingView;
            }

            // Создаем новую запись о просмотре
            var storyView = new StoryView
            {
                StoryId = storyId,
                ViewerId = viewerId,
                ViewedAt = DateTime.UtcNow
            };

            await _context.StoryViews.AddAsync(storyView);
            await _context.SaveChangesAsync();
            return storyView;
        }

        public async Task<bool> DeleteStoryAsync(int id, int authorId)
        {
            var story = await _context.Stories
                .FirstOrDefaultAsync(s => s.Id == id && s.AuthorId == authorId);

            if (story == null)
                return false;

            _context.Stories.Remove(story);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> CleanExpiredStoriesAsync()
        {
            var now = DateTime.UtcNow;
            var expiredStories = await _context.Stories
                .Where(s => s.ExpiresAt <= now)
                .ToListAsync();

            if (expiredStories.Any())
            {
                _context.Stories.RemoveRange(expiredStories);
                await _context.SaveChangesAsync();
            }

            return expiredStories.Count;
        }
    }
}
