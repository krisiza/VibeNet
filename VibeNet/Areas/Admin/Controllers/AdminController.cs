using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VibeNet.Core.Interfaces;
using VibeNet.Core.Services;
using VibeNet.Core.ViewModels;
using VibeNet.Extensions;
using static VibeNet.Infrastucture.Constants.AdminConstant;
using static VibeNetInfrastucture.Constants.Validations;

namespace VibeNet.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AminRole)]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IVibeNetService vibeNetService;
        public AdminController(UserManager<IdentityUser> userManager, IVibeNetService vibeNetService)
        {
            this.userManager = userManager;
            this.vibeNetService = vibeNetService;
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

        public async Task<IActionResult> DeleteUser(string userId)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
