using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VibeNet.Attributes;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.Services;
using VibeNet.Core.ViewModels;
using VibeNet.Extensions;

namespace VibeNet.Controllers
{
    public class UserController : BaseController
    {
        private readonly IVibeNetService vibeNetService;
        private readonly IIdentityUserService identityUserService;
        private readonly IProfilePictureService profilePictureService;
        private readonly IFriendshiprequestService friendshiprequestService;
        private readonly IFriendshipService friendshipService;

        public UserController(IVibeNetService vibeNetService, IIdentityUserService identityUserService,
            IProfilePictureService profilePictureService, IFriendshiprequestService friendshiprequestService,
            IFriendshipService friendshipService)
        {
            this.vibeNetService = vibeNetService;
            this.identityUserService = identityUserService;
            this.profilePictureService = profilePictureService;
            this.friendshiprequestService = friendshiprequestService;
            this.friendshipService = friendshipService;
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            VibeNetUserRegisterViewModel model = new();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromForm] VibeNetUserRegisterViewModel model)
        {
            var userId = User.Id();
            model.Id = Guid.Parse(userId);

            if (ModelState.IsValid)
            {
                await vibeNetService.AddUserAsync(model);
                return RedirectToAction("ShowProfile", "User", new { userId = userId });
            }

            await identityUserService.DeleteIdentityUserAsync(model.Id);
            return View(model);
        }

        [HttpGet]
        [NotExistingUser]
        public async Task<IActionResult> ShowProfile(string userId)
        {
            VibeNetUserProfileViewModel model = await vibeNetService.CreateVibeNetUserProfileViewModel(userId);

            if (model == null) return BadRequest();

            if (model.ProfilePicture != null)
            {
                ViewBag.Base64String = $"data:{model.ProfilePicture.ContentType};base64," + Convert.ToBase64String(model.ProfilePicture.Data, 0, model.ProfilePicture.Data.Length);
            }

            return View(model);
        }

        [NotExistingUser]
        public async Task<IActionResult> SendFriendRequest(string userId)
        {
            if (User.Id() == null) return BadRequest();

            string recipient = User.Id();

            if (await friendshipService.FindByIdAsync(userId, recipient))
            {
                TempData["AlertMessage"] = "The user is already your friend!";
            }
            else if (await friendshiprequestService.FindByIdAsync(recipient, userId))
            {
                TempData["AlertMessage"] = "This user has already sent you a friend request!";
            }
            else if (await friendshiprequestService.FindByIdAsync(userId, recipient))
            {
                TempData["AlertMessage"] = "Friendrequest already sent!";
            }
            else
            {
                await friendshiprequestService.SendRequestAsync(userId, recipient);
                TempData["AlertMessage"] = "Friend request sent successfully!";
            }


            return RedirectToAction("ShowProfile", "User", new { userId = userId });
        }
    }
}
