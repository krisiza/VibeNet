using Microsoft.AspNetCore.Mvc;
using VibeNet.Core.Contracts;
using VibeNet.Core.Services;

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

        public async Task<IActionResult> Delete(string userId)
        {
            await friendshipService.DeleteAsync(userId);
            var friends = await friendshipService.GetFriendsAsync(User.Id());
            return View("ShowFriends", friends);
        }
    }
}
