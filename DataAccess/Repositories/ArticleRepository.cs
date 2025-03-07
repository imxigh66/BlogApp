using Application.Abstractions;
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

        public async Task<Article> GetArticleById(int id)
        {
            return await _context.Articles.Include(a => a.Author).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<ICollection<Article>> GetAllArticle()
        {
            return await _context.Articles.Include(a => a.Author).ToListAsync();
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

        
        
    }

}
