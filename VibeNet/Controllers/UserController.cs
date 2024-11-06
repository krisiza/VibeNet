using Microsoft.AspNetCore.Mvc;
using VibeNet.ViewModels;

namespace VibeNet.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult RegisterUser()
        {
            VibeNetUserRegisterViewModel model = new();

            return View(model);
        }

        [HttpPost]
        public IActionResult RegisterUser(VibeNetUserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
       
            }
            return RedirectToAction(nameof(ShowProfile));
        }
        
        public IActionResult ShowProfile()
        {

            return View();
        }
    }
}
