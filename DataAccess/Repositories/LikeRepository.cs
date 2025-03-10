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
    public class LikeRepository : ILikeRepository
    {
        private readonly BlogDbContext _context;

        public LikeRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<Like> GetByArticleAndIpAsync(int articleId, string ipAddress)
        {
            return await _context.Likes
                .FirstOrDefaultAsync(l => l.ArticleId == articleId && l.ClientIpAddress == ipAddress);
        }

        public async Task<List<Like>> GetByArticleIdAsync(int articleId)
        {
            return await _context.Likes
                .Where(l => l.ArticleId == articleId)
                .ToListAsync();
        }

        public async Task<Like> AddAsync(Like like)
        {
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();
            return like;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like == null)
                return false;

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetLikeCountForArticleAsync(int articleId)
        {
            return await _context.Likes.CountAsync(l => l.ArticleId == articleId);
        }
    }
}
