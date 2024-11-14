using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        public PostController(IPostService postservice, IVibeNetService vibeNetService,
            ICommentService commentservice, ILikeService likeService)
        {
            this.postservice = postservice;
            this.vibeNetService = vibeNetService;
            this.commentservice = commentservice;
            this.likeService = likeService;
        }

        [NotExistingUser]
        public async Task<IActionResult> AllPosts(string userId)
        {
            var vibenetEntity = await vibeNetService.GetByIdentityIdAsync(userId);
            var model = await vibeNetService.CreateVibeNetUserProfileViewModel(userId);
            model.Posts = await postservice.GetAllAsync(userId);

            if (model.ProfilePicture != null)
            {
                ViewBag.Base64String = $"data:{model.ProfilePicture.ContentType};base64," +
                                       Convert.ToBase64String(model.ProfilePicture.Data, 0,
                                           model.ProfilePicture.Data.Length);
            }

            return View(model.Posts);
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

            if(await likeService.AddLikeAsync(postId, likeViewModel, userId))
                TempData["AlertMessage"] = "You liked this post";
            else
                TempData["AlertMessage"] = "Post is already liked";

            return RedirectToAction("AllPosts", "Post", new { userId = userId });
        }
    }
}