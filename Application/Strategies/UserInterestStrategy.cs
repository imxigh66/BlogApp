using Application.Abstractions;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Strategies
{
    public class UserInterestStrategy : IPostSortingStrategy
    {
        private readonly IUserActivityRepository _userActivityRepository;

        public UserInterestStrategy(IUserActivityRepository userActivityRepository)
        {
            _userActivityRepository = userActivityRepository;
        }

        public IQueryable<Article> Sort(IQueryable<Article> articles, int? userId = null)
        {
            if (!userId.HasValue)
            {
                // Если пользователь не авторизован, возвращаем по новизне
                return articles.OrderByDescending(a => a.CreatedAt);
            }

            // Получаем теги и авторов, которыми интересуется пользователь
            var userInterests = _userActivityRepository.GetUserInterests(userId.Value).Result;

            // Если у пользователя нет интересов, возвращаем по популярности
            if (userInterests == null || (userInterests.FavoriteAuthors.Count == 0 && userInterests.FavoriteTags.Count == 0))
            {
                return articles.OrderByDescending(a => a.Likes.Count + a.Comments.Count * 2);
            }

            // Сортируем статьи с учетом интересов пользователя
            return articles.OrderByDescending(a =>
    // Подсчитываем релевантность статьи для пользователя
    (userInterests.FavoriteAuthors.Contains(a.AuthorId) ? 10 : 0) +
    // Проверяем, что у статьи есть теги перед использованием
    ((a.Tags != null) ?
        a.Tags.Count(t => userInterests.FavoriteTags.Contains(t.Id)) * 5 : 0) +
    // Также учитываем новизну и популярность с меньшим весом
    (a.Likes.Count + a.Comments.Count) * 0.5 +
    (30 - (DateTime.UtcNow - a.CreatedAt).TotalDays) * 0.2
);
        }

        public string GetName() => "По интересам";
    }
}
