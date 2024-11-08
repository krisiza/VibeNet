using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VibeNet.Core.Contracts;
using VibeNet.Core.ViewModels;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post, int> postRepository;
        private readonly IRepository<Comment, int> commentRepository;
        private readonly IRepository<VibeNetUser, int> vibeNetUserRepository;

        public PostService(IRepository<Post, int> postRepository, IRepository<Comment, int> commentRepository,
            IRepository<VibeNetUser, int> vibeNetUserRepository)
        {
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.vibeNetUserRepository = vibeNetUserRepository;
        }

        public List<PostViewModel> GetAll(string userId)
            => postRepository.GetAllAttached()
                .Include(p => p.Comments)
                .Where(p => p.OwnerId == userId && p.IsDeleted == false)
                .Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    OwnerIdentityId = p.OwnerId,
                    Content = p.Content,
                    PostedOn = p.PostedOn,
                    IsDeleted = p.IsDeleted,
                    Comments = commentRepository.GetAllAttached()
                                .Where(c => c.PostId == p.Id && c.IsDeleted == false)
                                .Select(c => new CommentViewModel()
                                {
                                    Id = c.Id,
                                    Content = c.Content,
                                    PostedOn = c.PostedOn,
                                    IsDeleted = c.IsDeleted,
                                    OwnerId = c.OwnerId
                                })
                                .ToList()

                })
                .ToList();

        public async Task AddPostAsync(string postContent, string userId)
        {
            Post post = new Post()
            {
                OwnerId = userId,
                Content = postContent,
                PostedOn = DateTime.Now,
                IsDeleted = false,
            };

            await postRepository.AddAsync(post);
        }
    }
}
