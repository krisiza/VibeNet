using Microsoft.AspNetCore.Identity;
using VibeNet.Core.ViewModels;

namespace VibeNet.Core.Contracts
{
    public interface IPostService
    {
        Task<List<PostViewModel>> GetAllAsync(string userId);

        Task AddPostAsync(string postContent, string userId);

        Task<PostViewModel> GetByIdAsync(int postId);

        Task<IList<PostViewModel>?> GetFriendsPostsAsync(string userId);
    }
}
