using VibeNet.Core.ViewModels;

namespace VibeNet.Core.Contracts
{
    public interface ILikeService
    {
        Task<bool> AddLikeAsync(int postId, LikeViewModel model, string userId);
    }
}
