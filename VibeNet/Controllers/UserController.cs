using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VibeNet.Attributes;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.ViewModels;
using VibeNet.Extensions;
using static VibeNet.Infrastucture.Constants.CustomClaims;

namespace VibeNet.Controllers
{
    public class UserController : BaseController
    {
        private readonly IVibeNetService vibeNetService;
        private readonly IProfilePictureService profilePictureService;
        private readonly IFriendshiprequestService friendshiprequestService;
        private readonly IFriendshipService friendshipService;
        private readonly UserManager<IdentityUser> userManager;

        public UserController(IProfilePictureService profilePictureService, IFriendshiprequestService friendshiprequestService,
            IFriendshipService friendshipService, UserManager<IdentityUser> userManager,
            IVibeNetService vibeNetService)
        {
            this.vibeNetService = vibeNetService;
            this.profilePictureService = profilePictureService;
            this.friendshiprequestService = friendshiprequestService;
            this.friendshipService = friendshipService;
            this.userManager = userManager;
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

                IdentityUserClaim<string> userClaim = new()
                {
                    ClaimType = UserFullNameClaim,
                    ClaimValue = $"{model.FirstName} {model.LastName}",
                    UserId = userId
                };

                var user = await userManager.FindByIdAsync(userClaim.UserId);
                if (user != null)
                {
                    await userManager.AddClaimAsync(user, new System.Security.Claims.Claim(userClaim.ClaimType, userClaim.ClaimValue));
                }

                return RedirectToAction("ShowProfile", "User", new { userId = userId });
            }

            var userIdentity = await userManager.FindByIdAsync(userId);
            await userManager.DeleteAsync(userIdentity);
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
