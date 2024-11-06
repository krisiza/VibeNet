using Microsoft.AspNetCore.Mvc;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.ViewModels;

namespace VibeNet.Controllers
{
    public class UserController : Controller
    {
        private IVibeNetService vibeNetService;
        private IIdentityUserService identityUserService;

        public UserController(IVibeNetService vibeNetService, IIdentityUserService identityUserService)
        {
            this.vibeNetService = vibeNetService;
            this.identityUserService = identityUserService;
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            VibeNetUserRegisterViewModel model = new();

            return View(model);
        }

        [HttpPost]
        public IActionResult RegisterUser(VibeNetUserRegisterViewModel model)
        {

            //IMapper not working
            if (ModelState.IsValid)
            {
                vibeNetService.AddUserAsync(model);
                return RedirectToAction(nameof(ShowProfile));
            }

            
            identityUserService.DeleteIdentityUserAsync(model.Id);
            return View(model);
        }
        
        public IActionResult ShowProfile()
        {
            return View();
        }
    }
}
