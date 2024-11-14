using Microsoft.AspNetCore.Mvc;
using VibeNet.Attributes;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
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

        public UserController(IVibeNetService vibeNetService, IIdentityUserService identityUserService,
            IProfilePictureService profilePictureService, IFriendshiprequestService friendshiprequestService)
        {
            this.vibeNetService = vibeNetService;
            this.identityUserService = identityUserService;
            this.profilePictureService = profilePictureService;
            this.friendshiprequestService = friendshiprequestService;
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

            if(await friendshiprequestService.FindByIdAsync(userId, User.Id()))
            {
                await  friendshiprequestService.SendRequestAsync(userId, User.Id());
                TempData["AlertMessage"] = "Friend request sent successfully!";
            }
            else
                TempData["AlertMessage"] = "Friend request already sent!";

            return RedirectToAction("ShowProfile", "User", new { userId = userId });
        }
    }
}
