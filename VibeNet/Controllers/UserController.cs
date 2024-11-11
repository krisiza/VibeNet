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

            VibeNetUserProfileViewModel model = await vibeNetService.CreateVibeNetUserProfileViewModel(userId);

            if (model.ProfilePicture != null)
            {
                ViewBag.Base64String = $"data:{model.ProfilePicture.ContentType};base64," + Convert.ToBase64String(model.ProfilePicture.Data, 0, model.ProfilePicture.Data.Length);
            }

            return View(model);
        }
    }
}
