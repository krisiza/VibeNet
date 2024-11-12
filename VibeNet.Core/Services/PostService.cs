using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.ViewModels;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post, int> postRepository;
        private readonly IRepository<Comment, int> commentRepository;
        private readonly IVibeNetService vibeNetService;

        public PostService(IRepository<Post, int> postRepository, IRepository<Comment, int> commentRepository,
          IVibeNetService vibeNetService)
        {
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.vibeNetService = vibeNetService;
        }
        public async Task<List<PostViewModel>> GetAllAsync(string userId)
        {
            var posts = await postRepository.GetAllAttached()
                .Include(p => p.Comments)
                .Include(p => p.Owner)
                .Where(p => p.OwnerId == userId && !p.IsDeleted)
                .ToListAsync();

            var postViewModels = new List<PostViewModel>();

            foreach (var post in posts)
            {
                var comments = new List<CommentViewModel>();
                foreach (var comment in post.Comments.Where(c => !c.IsDeleted))
                {
                    var ownerProfile = await vibeNetService.CreateVibeNetUserProfileViewModel(comment.OwnerId);
                    comments.Add(new CommentViewModel
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        PostedOn = comment.PostedOn,
                        IsDeleted = comment.IsDeleted,
                        Owner = ownerProfile,
                    });
                }

                var postViewModel = new PostViewModel
                {
                    Id = post.Id,
                    OwnerId = post.OwnerId,
                    Content = post.Content,
                    PostedOn = post.PostedOn,
                    IsDeleted = post.IsDeleted,
                    Comments = comments
                };

                postViewModels.Add(postViewModel);
            }

            return postViewModels;
        }

        public async Task<PostViewModel> GetByIdAsync(int postId)
        {
           var post =  await postRepository.GetByIdAsync(postId);

            var comments = new List<CommentViewModel>();
            foreach (var comment in post.Comments.Where(c => !c.IsDeleted))
            {
                var ownerProfile = await vibeNetService.CreateVibeNetUserProfileViewModel(comment.OwnerId);
                comments.Add(new CommentViewModel
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    PostedOn = comment.PostedOn,
                    IsDeleted = comment.IsDeleted,
                    Owner = ownerProfile,
                });
            }

            var postViewModel = new PostViewModel
            {
                Id = post.Id,
                Content = post.Content,
                PostedOn = post.PostedOn,
                IsDeleted = post.IsDeleted,
                Comments = comments,
                OwnerId = post.OwnerId,
            };

            return postViewModel;
        }


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
