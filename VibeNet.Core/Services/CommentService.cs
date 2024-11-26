using VibeNet.Core.Contracts;
using VibeNet.Core.ViewModels;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment, int> commentRepository;
        private readonly IPostService postService;
        public CommentService(IRepository<Comment, int> commentRepository, IPostService postService)
        {
            this.commentRepository = commentRepository;
            this.postService = postService;
        }

        public async Task AddCommentAsync(int postId, CommentViewModel model, string userId)
        {
            var postEntity = await postService.GetByIdAsync(postId);

            var comment = new Comment()
            {
                PostId = postId,
                OwnerId = userId,
                Content = model.Content,
                PostedOn = model.PostedOn,
                IsDeleted = model.IsDeleted,
            };

            commentRepository.Add(comment);
        }

        public async Task DeleteAsync(string userId)
        {
            var comments = commentRepository.GetAllAttached()
                .Where(c => c.OwnerId == userId);

            if (comments == null) return;
            await commentRepository.DeleteEntityRangeAsync(comments);
        }
    }
}
