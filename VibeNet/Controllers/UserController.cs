using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using VibeNet.Attributes;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.Utilities;
using VibeNet.Core.ViewModels;
using VibeNetInfrastucture.Data.Models;
using static VibeNet.Infrastucture.Constants.CustomClaims;
using static VibeNetInfrastucture.Constants.Validations;

namespace VibeNet.Controllers
{
    public class UserController : BaseController
    {
        private readonly IVibeNetService vibeNetService;
        private readonly IProfilePictureService profilePictureService;
        private readonly IFriendshiprequestService friendshiprequestService;
        private readonly IFriendshipService friendshipService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IPictureHelper _pictureHelper;

        public UserController(IProfilePictureService profilePictureService, IFriendshiprequestService friendshiprequestService,
            IFriendshipService friendshipService, UserManager<IdentityUser> userManager,
            IVibeNetService vibeNetService, IPictureHelper pictureHelper)
        {
            this.vibeNetService = vibeNetService;
            this.profilePictureService = profilePictureService;
            this.friendshiprequestService = friendshiprequestService;
            this.friendshipService = friendshipService;
            this.userManager = userManager;
            this._pictureHelper = pictureHelper;  
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            VibeNetUserFormViewModel model = new();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromForm] VibeNetUserFormViewModel model)
        {
            var userId = User.Id();
            model.Id = Guid.Parse(userId);

            if (ModelState.IsValid)
            {
                try
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
                }
                catch (Exception ex)
                {
                    var userIden = await userManager.FindByIdAsync(userId);
                    await userManager.DeleteAsync(userIden);
                    return View(model);
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

        [NotExistingUser]
        [HttpGet]
        public async Task<IActionResult> EditProfile(string userId)
        {
            var model = await vibeNetService.CreateFormUserViewModel(userId);
            return View(model);
        }

        [NotExistingUser]
        [HttpPost]
        public async Task<IActionResult> EditProfile([FromForm] VibeNetUserFormViewModel model)
        {
            VibeNetUser? vibeNetUser = await vibeNetService.GetByIdentityIdAsync(model.Id.ToString());

            if (vibeNetUser == null) return View(model);

            vibeNetUser.FirstName = model.FirstName;
            vibeNetUser.LastName = model.LastName;
            vibeNetUser.HomeTown = model.HomeTown;
            vibeNetUser.Birthday = DateTime.ParseExact(model.Birthday, DateTimeFormat.Format, CultureInfo.InvariantCulture);
            vibeNetUser.Gender = model.Gender;

            if (model.ProfilePictureFile != null)
            {
                byte[] data = await _pictureHelper.ConvertToBytesAsync(model.ProfilePictureFile);
                vibeNetUser.ProfilePicture = await profilePictureService.SavePictureAsync(model.ProfilePictureFile, data);
            }
            await vibeNetService.UpdateAsync(vibeNetUser);

            return RedirectToAction("ShowProfile", "User", new { userId = vibeNetUser.IdentityUserId });
        }

        [HttpPost]
        public async Task<IActionResult> Searched(string searchTerm, string category, int pageNumber = 1, int pageSize = 4)
        {
            var (users, totalCount) = await vibeNetService.FindUsers(searchTerm, category, User.Id(), pageNumber, pageSize);

            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.PageSize = pageSize;

            return View(users);
        }
    }
}
