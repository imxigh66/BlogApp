using Application.Abstractions;
using DataAccess.Services;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly BlogDbContext _context;

        public ArticleRepository(BlogDbContext context)
        {
            _context = context;
        }


        
        public async Task<Article> UpdateArticleWithState(Article article)
        {
            var existingArticle = await _context.Articles.FindAsync(article.Id);
            if (existingArticle != null)
            {
                _context.Entry(existingArticle).CurrentValues.SetValues(article);

                // Обновляем поля состояния
                existingArticle.StateName = article.StateName;
                existingArticle.StateReason = article.StateReason ?? string.Empty;
                existingArticle.IsPublished = article.IsPublished;

                await _context.SaveChangesAsync();

                // Очищаем кэш
                string cacheKey = $"article_{article.Id}";
                CacheService.Instance.Remove(cacheKey);
            }
            return existingArticle;
        }
        public async Task<Article> GetArticleById(int id)
        {
            // Пытаемся получить из кэша
            string cacheKey = $"article_{id}";
            if (CacheService.Instance.TryGetValue(cacheKey, out Article cachedArticle))
            {
                return cachedArticle;
            }

            // Если нет в кэше, запрашиваем из базы
            var article = await _context.Articles
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => a.Id == id);

            // Если нашли статью, проверяем и устанавливаем значения по умолчанию при необходимости
            if (article != null)
            {
                // Устанавливаем значения по умолчанию, если они NULL
                if (article.StateName == null)
                {
                    article.StateName = article.IsPublished ? "Опубликована" : "Черновик";
                }

                if (article.StateReason == null)
                {
                    article.StateReason = string.Empty;
                }

                // Инициализируем состояние
                article.GetState();

                // Кэшируем
                CacheService.Instance.Set(cacheKey, article, TimeSpan.FromMinutes(10));
            }

            return article;
        }



        public async Task<ICollection<Article>> GetAllArticle()
        {
            return await _context.Articles.Include(a => a.Author).Include(a => a.ContentItems).ToListAsync();
        }

        public async Task<Article> AddArticle(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task<Article> UpdateArticle(string updatedContent, int articalId)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(p => p.Id == articalId);
            article.Content = updatedContent;
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<ICollection<Article>> GetPendingArticles()
        {
            return await _context.Articles
                .Where(a => !a.IsPublished)
                .Include(a => a.Author)
                .ToListAsync();
        }

        public async Task UpdateArticleStatus(int articleId, bool isPublished)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article != null)
            {
                article.IsPublished = isPublished;
                await _context.SaveChangesAsync();
            }
        }



    }

}
