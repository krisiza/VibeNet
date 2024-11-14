using Microsoft.AspNetCore.Mvc;

namespace VibeNet.Controllers
{
    public class FriendrequestController : BaseController
    {
        public IActionResult ShowFriendrequests()
        {
            return View();
        }
    }
}
