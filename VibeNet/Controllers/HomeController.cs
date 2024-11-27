using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using VibeNet.Models;

namespace VibeNet.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Index(string userId)
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                if (!string.IsNullOrEmpty(userId))
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
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
