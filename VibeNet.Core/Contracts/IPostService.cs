using VibeNet.Core.ViewModels;

namespace VibeNet.Core.Contracts
{
    public interface IPostService
    {
        List<PostViewModel> GetAll(string userId);

        Task AddPostAsync(string postContent, string userId);
    }
}
