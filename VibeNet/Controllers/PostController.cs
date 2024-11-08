using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Execution;
using System.Security.Claims;
using VibeNet.Core.Contracts;
using VibeNet.Core.ViewModels;

namespace VibeNet.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService postservice;

        public PostController(IPostService postservice)
        {
            this.postservice = postservice;
        }
        public async Task<IActionResult> AllPosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var posts = postservice.GetAll(userId);

            return View(posts);
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
