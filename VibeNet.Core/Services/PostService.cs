using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.ViewModels;
using VibeNet.Infrastucture.Data.Models;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post, int> postRepository;
        private readonly IRepository<Comment, int> commentRepository;
        private readonly IRepository<Like, int> likeRepositoty;
        private readonly IVibeNetService vibeNetService;
        private readonly IFriendshipService friendshipService;
        private readonly UserManager<IdentityUser> userManager;

        public PostService(IRepository<Post, int> postRepository, IRepository<Comment, int> commentRepository,
          IVibeNetService vibeNetService, UserManager<IdentityUser> userManager, IFriendshipService friendshipService,
         IRepository<Like, int> likeRepositoty)
        {
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.vibeNetService = vibeNetService;
            this.userManager = userManager;
            this.friendshipService = friendshipService;
            this.likeRepositoty = likeRepositoty;
        }
        public async Task<List<PostViewModel>> GetAllAsync(string userId)
        {
            var posts = await postRepository.GetAllAttached()
                .Include(p => p.Comments)
                .Include(p => p.Owner)
                .Include(p => p.UserLiked)
                .Where(p => p.OwnerId == userId)
                .ToListAsync();

            var postViewModels = new List<PostViewModel>();

            foreach (var post in posts)
            {
                var comments = new List<CommentViewModel>();
                foreach (var comment in post.Comments)
                {
                    var ownerProfile = await vibeNetService.CreateVibeNetUserProfileViewModel(comment.OwnerId);
                    comments.Add(new CommentViewModel
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        PostedOn = comment.PostedOn,
                        Owner = ownerProfile,
                    });
                }

                var likes = new List<LikeViewModel>();
                foreach (var like in post.UserLiked)
                {
                    var ownerProfile = await vibeNetService.CreateVibeNetUserProfileViewModel(like.OwnerId);
                    likes.Add(new LikeViewModel
                    {
                        Id = like.Id,
                        Owner = ownerProfile,
                    });
                }

                var postViewModel = new PostViewModel
                {
                    Id = post.Id,
                    OwnerId = post.OwnerId,
                    Owner = await vibeNetService.CreateVibeNetUserProfileViewModel(post.OwnerId),
                    Content = post.Content,
                    PostedOn = post.PostedOn,
                    UserLiked = likes,
                    Comments = comments
                };

                postViewModels.Add(postViewModel);
            }

            return postViewModels;
        }

        public async Task<IList<PostViewModel>?> GetFriendsPostsAsync(string userId)
        {
            var friends = await friendshipService.GetFriendsAsync(userId);
            var postViewModels = new List<PostViewModel>();

            if (!friends.Any()) return null;

            var posts = await postRepository.GetAllAttached()
               .Include(p => p.Comments)
               .Include(p => p.Owner)
               .Include(p => p.UserLiked)
               .ToListAsync();

            if (posts == null) return null;

            posts = posts
                .Where(p => friends.Any(f => f.IdentityId == p.OwnerId))
                .ToList();

            foreach (var post in posts)
            {
                var comments = new List<CommentViewModel>();
                foreach (var comment in post.Comments)
                {
                    var ownerProfile = await vibeNetService.CreateVibeNetUserProfileViewModel(comment.OwnerId);
                    comments.Add(new CommentViewModel
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        PostedOn = comment.PostedOn,
                        Owner = ownerProfile,
                    });
                }

                var likes = new List<LikeViewModel>();
                foreach (var like in post.UserLiked)
                {
                    var ownerProfile = await vibeNetService.CreateVibeNetUserProfileViewModel(like.OwnerId);
                    likes.Add(new LikeViewModel
                    {
                        Id = like.Id,
                        Owner = ownerProfile,
                    });
                }

                var postViewModel = new PostViewModel
                {
                    Id = post.Id,
                    OwnerId = post.OwnerId,
                    Owner = await vibeNetService.CreateVibeNetUserProfileViewModel(post.OwnerId),
                    Content = post.Content,
                    PostedOn = post.PostedOn,
                    UserLiked = likes,
                    Comments = comments
                };

                postViewModels.Add(postViewModel);
            }

            return postViewModels;
        }

        public async Task<PostViewModel> GetByIdAsync(int postId)
        {
            var post = await postRepository.GetByIdAsync(postId);

            var comments = new List<CommentViewModel>();
            foreach (var comment in post.Comments)
            {
                var ownerProfile = await vibeNetService.CreateVibeNetUserProfileViewModel(comment.OwnerId);
                comments.Add(new CommentViewModel
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    PostedOn = comment.PostedOn,
                    Owner = ownerProfile,
                });
            }

            var postViewModel = new PostViewModel
            {
                Id = post.Id,
                Content = post.Content,
                PostedOn = post.PostedOn,
                Comments = comments,
                OwnerId = post.OwnerId,
                Owner = await vibeNetService.CreateVibeNetUserProfileViewModel(post.OwnerId),
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
            };

            await postRepository.AddAsync(post);
        }

        public async Task DeleteAllAsync(string userId)
        {
            var posts = await postRepository.GetAllAttached()
                .Include(p => p.Comments)
                .Include(p => p.Owner)
                .Include(p => p.UserLiked)
                .Where(p => p.OwnerId == userId)
                .ToListAsync();

            foreach (var post in posts)
            {
                await likeRepositoty.DeleteEntityRangeAsync(post.UserLiked);
            }

            foreach (var post in posts)
            {
                await commentRepository.DeleteEntityRangeAsync(post.Comments);
            }

            if (posts == null) return;
            await postRepository.DeleteEntityRangeAsync(posts);
        }

        public async Task DeleteAsync(PostViewModel model)
        {
            var post = await postRepository.GetAllAttached()
                .Include(p => p.Comments)
                .Include(p => p.UserLiked)
                .Where(p => p.Id == model.Id)
                .FirstOrDefaultAsync();

            if (post == null) return;

            await likeRepositoty.DeleteEntityRangeAsync(post.UserLiked);
            await commentRepository.DeleteEntityRangeAsync(post.Comments);
            await postRepository.DeleteEntityAsync(post);
        }
    }
}
