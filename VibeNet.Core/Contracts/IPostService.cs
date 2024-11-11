using VibeNet.Core.ViewModels;

namespace VibeNet.Core.Contracts
{
    public interface IPostService
    {
        Task<List<PostViewModel>> GetAllAsync(string userId);

        Task AddPostAsync(string postContent, string userId);
    }
}
