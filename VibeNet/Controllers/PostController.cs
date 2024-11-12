using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.ViewModels;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("ShowProfile", "User");

            //Add Validation
            await postservice.AddPostAsync(postContent, userId);

            return RedirectToAction(nameof(AllPosts));
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(string ownerId,int postId, string commentContent)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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