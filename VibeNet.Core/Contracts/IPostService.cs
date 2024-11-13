using VibeNet.Core.ViewModels;

namespace VibeNet.Core.Contracts
{
    public interface IPostService
    {
        Task<List<PostViewModel>> GetAllAsync(string userId);

        public PostViewModel CreatePost(string? postContent, string userId);

        Task AddPostAsync(PostViewModel post);

        Task<PostViewModel> GetByIdAsync(int postId);
    }
}
