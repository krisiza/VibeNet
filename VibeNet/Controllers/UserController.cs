using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.Utilities;
using VibeNet.Core.ViewModels;
using static VibeNetInfrastucture.Validations.ValidationConstants;

namespace VibeNet.Controllers
{
    public class UserController : BaseController
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
        public async Task<IActionResult> RegisterUser([FromForm] VibeNetUserRegisterViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.Id = Guid.Parse(userId);

            if (ModelState.IsValid)
            {
                await vibeNetService.AddUserAsync(model);

                if (userId == null) return View(model);
                var user = await vibeNetService.GetByIdentityIdAsync(userId);

                byte[] data = await VibeNetHepler.ConvertToBytesAsync(model.ProfilePictureFile);
                await profilePictureService.SavePicture(model.ProfilePictureFile, user.Id, data);
                var profilePicture = await profilePictureService.GetProfilePictureAsync(user.Id);

                if (profilePicture != null)
                {
                    user.ProfilePictureId = profilePicture.Id;
                    await vibeNetService.UpdateAsync(user);
                }

                return RedirectToAction(nameof(ShowProfile));
            }

            await identityUserService.DeleteIdentityUserAsync(model.Id);
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
    }
}
