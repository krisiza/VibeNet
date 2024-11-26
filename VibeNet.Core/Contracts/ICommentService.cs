using VibeNet.Core.ViewModels;

namespace VibeNet.Core.Contracts
{
    public interface ICommentService
    {
        Task AddCommentAsync(int postId, CommentViewModel model, string userId);
        Task DeleteAsync(string userId);
    }
}
