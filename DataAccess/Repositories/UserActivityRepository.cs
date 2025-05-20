using Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserActivityRepository : IUserActivityRepository
    {
        private readonly BlogDbContext _context;

        public UserActivityRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<UserInterests> GetUserInterests(int userId)
        {
            // Получаем статьи, которые лайкнул пользователь
            var likedArticleIds = await _context.Likes
                .Where(l => l.Article.Author.Id == userId)
                .Select(l => l.ArticleId)
                .ToListAsync();

            // Получаем авторов этих статей
            var favoriteAuthors = await _context.Articles
                .Where(a => likedArticleIds.Contains(a.Id))
                .Select(a => a.AuthorId)
                .Distinct()
                .ToListAsync();

            // В реальной системе здесь также был бы код для получения тегов
            // Для упрощения просто вернем пустой список
            var favoriteTags = new List<int>();

            return new UserInterests
            {
                FavoriteAuthors = favoriteAuthors,
                FavoriteTags = favoriteTags
            };
        }
    }
}
