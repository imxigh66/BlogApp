using Application.Strategies;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IArticleRepository
    {
        Task<Article> GetArticleById(int id);
       // Task<ICollection<Article>> GetAllArticle();
        Task<Article> AddArticle(Article article);
        Task<Article> UpdateArticle(string  article,int articleId);
        Task DeleteArticle(int id);
        Task<ICollection<Article>> GetPendingArticles();
        Task UpdateArticleStatus(int articleId, bool isPublished);

        Task<Article> UpdateArticleWithState(Article article);
        Task<ICollection<Article>> GetAllArticles(IPostSortingStrategy sortingStrategy, int? userId = null);
    }
}


