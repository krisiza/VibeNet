using Microsoft.AspNetCore.Mvc;
using VibeNet.Core.Contracts;
using VibeNet.Extensions;

namespace VibeNet.Controllers
{
    public class FriendrequestController : BaseController
    {
        private readonly IFriendshiprequestService friendshiprequestService;

        public FriendrequestController(IFriendshiprequestService friendshiprequestService)
        {
            this.friendshiprequestService = friendshiprequestService;
        }

        public async Task<IActionResult> ShowFriendrequests(string userId)
        {
            if (userId == User.Id())
                return View(await friendshiprequestService.GetFriendrequets(userId));

            return BadRequest();
        }

        public async Task<IActionResult> Confirm(string userId)
        {
            await friendshiprequestService.AcceptRequest(userId, User.Id());
            return RedirectToAction("ShowFriendrequests", "Friendrequest", new { userId = User.Id() });
        }

        public async Task<IActionResult> Delete(string userId)
        {

            await friendshiprequestService.DeleteRequestAsync(userId, User.Id());
            return RedirectToAction("ShowFriendrequests", "Friendrequest", new { userId = User.Id() });
        }
    }
}
