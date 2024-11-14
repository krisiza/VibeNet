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
        private readonly IVibeNetService vibeNetService;

        public PostController(IPostService postservice, ICommentService commentservice, IVibeNetService vibeNetService)
        {
            this.postservice = postservice;
            this.vibeNetService = vibeNetService;
            this.commentservice = commentservice;
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
    }
}