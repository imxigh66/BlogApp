using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IStoryRepository
    {
        Task<Story> CreateStoryAsync(Story story);
        Task<Story> GetStoryByIdAsync(int id);
        Task<List<Story>> GetActiveStoriesByUserAsync(int userId);
        Task<List<Story>> GetActiveStoriesForFeedAsync(int viewerId, int count = 20);
        Task<StoryView> AddStoryViewAsync(int storyId, int viewerId);
        Task<bool> DeleteStoryAsync(int id, int authorId);
        Task<int> CleanExpiredStoriesAsync();
    }
}
