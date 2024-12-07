using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.ViewModels;
using static VibeNet.Infrastucture.Constants.AdminConstant;

namespace VibeNet.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AminRole)]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IVibeNetService vibeNetService;
        private readonly IPostService postService;
        private readonly IProfilePictureService profilePictureService;
        private readonly ICommentService commentService;
        private readonly ILikeService likeService;
        private readonly IFriendshipService friendshipService;
        private readonly IFriendshiprequestService friendshiprequestService;
        public AdminController(UserManager<IdentityUser> userManager, IVibeNetService vibeNetService,
            IPostService postService, ICommentService commentService,
            ILikeService likeService, IFriendshipService friendshipService, IFriendshiprequestService friendshiprequestService,
            IProfilePictureService profilePictureService)
        {
            this.userManager = userManager;
            this.vibeNetService = vibeNetService;
            this.postService = postService;
            this.commentService = commentService;
            this.likeService = likeService;
            this.friendshipService = friendshipService;
            this.friendshiprequestService = friendshiprequestService;
            this.profilePictureService = profilePictureService;
        }

        [Route("Admin/Admin/Index")]
        public async Task<IActionResult> Index()
        {
            var users = userManager.Users.ToList();

            List<VibeNetUserProfileViewModel> models = new List<VibeNetUserProfileViewModel>();

            if (users == null) return View(null);

            foreach (var user in users)
            {
                if (user.Id == User.Id()) continue;
                var model = await vibeNetService.CreateVibeNetUserProfileViewModel(user.Id);
                models.Add(model);
            }

            return View(models);
        }

        [Route("Admin/Admin/Delete/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            await likeService.DeleteAsync(userId);
            await commentService.DeleteAsync(userId);
            await postService.DeleteAllAsync(userId);
            await friendshiprequestService.DeleteAsync(userId);
            await friendshipService.DeleteAsync(userId);
            await vibeNetService.DeleteAsync(userId);
            var user = (await userManager.FindByIdAsync(userId));
            await userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }

        [Route("Admin/Admin/DeletePost/{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var post = await postService.GetByIdAsync(postId);
            await postService.DeleteAsync(post);

            return RedirectToAction("AllPosts", "Post", new { userId = post.OwnerId });
        }
    }
}
