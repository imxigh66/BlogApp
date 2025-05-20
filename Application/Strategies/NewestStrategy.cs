using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Strategies
{
    public class NewestStrategy : IPostSortingStrategy
    {
        public IQueryable<Article> Sort(IQueryable<Article> articles, int? userId = null)
        {
            // Сортируем статьи по дате создания (от новых к старым)
            return articles.OrderByDescending(a => a.CreatedAt);
        }

        public string GetName() => "По новизне";
    }
}
