using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using VibeNet.Core.Interfaces;
using VibeNet.Core.Services;
using VibeNet.Models;

namespace VibeNet.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IVibeNetService vibeNetService)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public  IActionResult Index(string userId)
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                if (userId != null)
                {
                    // Redirect to the ShowProfile action with the username in the URL
                    return RedirectToAction("ShowProfile", "User", new { userId = userId });
                }
                else
                {
                    var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    return RedirectToAction("ShowProfile", "User", new { userId = identityId });
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
