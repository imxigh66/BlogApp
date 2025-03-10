using Domain.Models;


namespace Application.Abstractions
{
    public interface ICommentRepository
    {
        Task<Comment> GetByIdAsync(int id);
        Task<List<Comment>> GetByArticleIdAsync(int articleId);
        Task<Comment> AddAsync(Comment comment);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCommentCountForArticleAsync(int articleId);
    }
}
