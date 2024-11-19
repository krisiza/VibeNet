using Microsoft.AspNetCore.Mvc;
using VibeNet.Attributes;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.ViewModels;
using VibeNet.Extensions;

namespace VibeNet.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostService postservice;
        private readonly ICommentService commentservice;
        private readonly ILikeService likeService;
        private readonly IVibeNetService vibeNetService;
        private readonly IFriendshipService friendshipService;

        public PostController(IPostService postservice, IVibeNetService vibeNetService,
            ICommentService commentservice, ILikeService likeService, IFriendshipService friendshipService)
        {
            this.postservice = postservice;
            this.vibeNetService = vibeNetService;
            this.commentservice = commentservice;
            this.likeService = likeService;
            this.friendshipService = friendshipService;
        }

        [NotExistingUser]
        public async Task<IActionResult> AllPosts(string userId)
        {
            var vibenetEntity = await vibeNetService.GetByIdentityIdAsync(userId);
            var model = await vibeNetService.CreateVibeNetUserProfileViewModel(userId);

            if (!await friendshipService.FindByIdAsync(User.Id(), userId) && !await friendshipService.FindByIdAsync(User.Id(), userId) && userId != User.Id())
            {
                TempData["AlertMessage"] = $"{model.FirstName} {model.LastName} is not your friend!";
                return RedirectToAction("ShowProfile", "User", new { userId = userId });
            }
            else
            {
                model.Posts = await postservice.GetAllAsync(userId);
                TempData["UserName"] = vibenetEntity.FirstName + " " + vibenetEntity.LastName;

                if (model.ProfilePicture != null)
                {
                    ViewBag.Base64String = $"data:{model.ProfilePicture.ContentType};base64," +
                                           Convert.ToBase64String(model.ProfilePicture.Data, 0,
                                               model.ProfilePicture.Data.Length);
                }
                return View(model.Posts);
            }

        }

        public async Task<IActionResult> ShowFeeds(string userId)
        {
            if(userId != User.Id()) return BadRequest();

            var vibenetEntity = await vibeNetService.GetByIdentityIdAsync(userId);
            var model = await vibeNetService.CreateVibeNetUserProfileViewModel(userId);

            ViewBag.Base64String = $"data:{model.ProfilePicture.ContentType};base64," +
                                          Convert.ToBase64String(model.ProfilePicture.Data, 0,
                                              model.ProfilePicture.Data.Length);

            TempData["UserName"] = vibenetEntity.FirstName + " " + vibenetEntity.LastName;

            var posts = await postservice.GetFriendsPostsAsync(userId);
            return View(posts);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string postContent)
        {
            var userId = User.Id();

            if (!ModelState.IsValid)
            {
                return RedirectToAction("ShowProfile", "User", new { userId = userId });
            }

            await postservice.AddPostAsync(postContent, userId);
            return RedirectToAction("AllPosts", "Post", new { userId = userId });
        }

        [HttpPost]
        [NotExistingUser]
        public async Task<IActionResult> AddComment(string ownerId, int postId, string commentContent)
        {
            var userId = User.Id();
            var userViewModel = await vibeNetService.CreateVibeNetUserProfileViewModel(userId);

            CommentViewModel commentViewModel = new CommentViewModel()
            {
                Content = commentContent,
                Owner = userViewModel,
                IsDeleted = false,
                PostedOn = DateTime.Now,
            };

            await commentservice.AddCommentAsync(postId, commentViewModel, userId);
            return RedirectToAction("AllPosts", "Post", new { userId = ownerId });
        }

        [NotExistingUser]
        public async Task<IActionResult> LikePost(string userId, int postId)
        {
            var likeOwner = User.Id();
            var userViewModel = await vibeNetService.CreateVibeNetUserProfileViewModel(likeOwner);

            LikeViewModel likeViewModel = new LikeViewModel()
            {
                Owner = userViewModel,
            };

            if (await likeService.AddLikeAsync(postId, likeViewModel, userId))
                TempData["AlertMessage"] = "You liked this post";
            else
                TempData["AlertMessage"] = "Post is already liked";

            return RedirectToAction("AllPosts", "Post", new { userId = userId });
        }
    }
}