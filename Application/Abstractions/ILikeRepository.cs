using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface ILikeRepository
    {
        Task<Like> GetByArticleAndIpAsync(int articleId, string ipAddress);
        Task<List<Like>> GetByArticleIdAsync(int articleId);
        Task<Like> AddAsync(Like like);
        Task<bool> DeleteAsync(int id);
        Task<int> GetLikeCountForArticleAsync(int articleId);
    }
}
