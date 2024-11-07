using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.Utilities;
using VibeNet.Core.ViewModels;

namespace VibeNet.Controllers
{
    public class UserController : Controller
    {
        private readonly IVibeNetService vibeNetService;
        private readonly IIdentityUserService identityUserService;
        private readonly IProfilePictureService profilePictureService;

        public UserController(IVibeNetService vibeNetService, IIdentityUserService identityUserService,
            IProfilePictureService profilePictureService)
        {
            this.vibeNetService = vibeNetService;
            this.identityUserService = identityUserService;
            this.profilePictureService = profilePictureService;
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            VibeNetUserRegisterViewModel model = new();
            return View(model);
        }

        [HttpPost]
        public IActionResult RegisterUser([FromForm] VibeNetUserRegisterViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return View(model);

            model.Id = Guid.Parse(userId);

            if (ModelState.IsValid)
            {
                vibeNetService.AddUserAsync(model);
                return RedirectToAction(nameof(ShowProfile));
            }

            identityUserService.DeleteIdentityUserAsync(model.Id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ShowProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction(nameof(ShowProfile));
            var user =await vibeNetService.GetByIdentityIdAsync(userId);

            VibeNetUserProfileViewModel model = new VibeNetUserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                HomeTown = user.HomeTown,
                Birthday = user.Birthday.ToString(),
            };

            var profilePicture = await profilePictureService.GetProfilePictureAsync(user.Id);

            if (profilePicture != null)
            {
                ViewBag.Base64String = $"data:{profilePicture.ContentType};base64," + Convert.ToBase64String(profilePicture.Data, 0, profilePicture.Data.Length);
                model.ProfilePicture = profilePicture;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ShowProfile([FromForm] VibeNetUserProfileViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction(nameof(ShowProfile));
            var user = await vibeNetService.GetByIdentityIdAsync(userId);
            byte[] data = await VibeNetHepler.ConvertToBytesAsync(model.ProfilePictureFile);
            await profilePictureService.SavePicture(model.ProfilePictureFile, user.Id, data);
            var profilePicture = await profilePictureService.GetProfilePictureAsync(user.Id);

            if (profilePicture != null)
            {
                user.ProfilePictureId = profilePicture.Id;
                await vibeNetService.UpdateAsync(user);
            }

            ViewBag.Base64String = $"data:{profilePicture.ContentType};base64," + Convert.ToBase64String(profilePicture.Data, 0, profilePicture.Data.Length);

            return View(model);
        }
    }
}
