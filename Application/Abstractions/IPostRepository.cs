using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IPostRepository
    {
        Task<ICollection<Post>> GetAllPosts();
        Task<Post> GetPostsById(int postId);
        Task<Post> CreatePost(Post toCreate);
        Task<Post> UpdatePost(string updatedContent,int postId);
        Task DeletePost(int postId);
    }
}
