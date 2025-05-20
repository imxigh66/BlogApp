using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Strategies
{
    public interface IPostSortingStrategy
    {
        // Метод для сортировки списка статей
        IQueryable<Article> Sort(IQueryable<Article> articles, int? userId = null);

        // Название стратегии (для логирования и отладки)
        string GetName();
    }
}
