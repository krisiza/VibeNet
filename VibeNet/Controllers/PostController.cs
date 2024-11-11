using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;

namespace VibeNet.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService postservice;
        private readonly IVibeNetService vibeNetService;

        public PostController(IPostService postservice, IVibeNetService vibeNetService)
        {
            this.postservice = postservice;
            this.vibeNetService = vibeNetService;
        }
        public async Task<IActionResult> AllPosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await vibeNetService.CreateVibeNetUserProfileViewModel(userId);
            model.Posts = await postservice.GetAllAsync(userId);

            if (model.ProfilePicture != null)
            {
                ViewBag.Base64String = $"data:{model.ProfilePicture.ContentType};base64," + Convert.ToBase64String(model.ProfilePicture.Data, 0, model.ProfilePicture.Data.Length);
            }

            return View(model.Posts);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string postContent)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("ShowProfile", "User");
            await postservice.AddPostAsync(postContent, userId);

            return RedirectToAction(nameof(AllPosts));
        }
    }
}
