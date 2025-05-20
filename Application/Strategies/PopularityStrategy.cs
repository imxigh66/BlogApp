using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Strategies
{
    public class PopularityStrategy : IPostSortingStrategy
    {
        public IQueryable<Article> Sort(IQueryable<Article> articles, int? userId = null)
        {
            // Сортируем статьи по популярности (кол-во лайков + кол-во комментариев)
            return articles
                .OrderByDescending(a => a.Likes.Count + a.Comments.Count * 2);
        }

        public string GetName() => "По популярности";
    }
}
