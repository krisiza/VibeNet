using Microsoft.AspNetCore.Mvc;
using VibeNet.Core.Contracts;
using VibeNet.Extensions;

namespace VibeNet.Controllers
{
    public class FriendShipController : Controller
    {
        private readonly IFriendshipService friendshipService;

        public FriendShipController(IFriendshipService friendshipService)
        {
            this.friendshipService = friendshipService;
        }

        public async Task<IActionResult> ShowFriends(string userId)
        {
            if (userId == User.Id())
                return View(await friendshipService.GetFriendsAsync(userId));


            return BadRequest();
        }
    }
}
