using VibeNet.Core.Contracts;
using VibeNet.Core.ViewModels;
using VibeNet.Infrastucture.Data.Models;
using VibeNet.Infrastucture.Repository.Contracts;

namespace VibeNet.Core.Services
{
    public class LikeService : ILikeService
    {
        private readonly IRepository<Like, int> likeRepository;
        private readonly IPostService postService;
        public LikeService(IRepository<Like, int> likeRepository, IPostService postService)
        {
            this.likeRepository = likeRepository;
            this.postService = postService;
        }

        public async Task<bool> AddLikeAsync(int postId, LikeViewModel model, string userId)
        {
            var postEntity = await postService.GetByIdAsync(postId);

            var like = new Like()
            {
                PostId = postId,
                OwnerId = model.Owner.IdentityId,
            };

            var likes = await likeRepository.GetAllAsync();

            bool postIsLiked = likes.Any(l => l.OwnerId == like.OwnerId && l.PostId == like.PostId);

            if (!postIsLiked)
            {
                await likeRepository.AddAsync(like);
                return true;
            }

            return false;
        }

        public async Task DeleteAsync(string userId)
        {
            var likes = likeRepository.GetAllAttached()
                .Where(l => l.OwnerId == userId);

            if (likes == null) return;

            await likeRepository.DeleteEntityRangeAsync(likes);
        }
    }
}
