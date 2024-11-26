using Microsoft.EntityFrameworkCore;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.ViewModels;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Core.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IRepository<Friendship, object> friendshipRepository;
        private readonly IVibeNetService vibeNetService;
        public FriendshipService(IRepository<Friendship, object> friendshipRepository, IVibeNetService vibeNetService)
        {
            this.friendshipRepository = friendshipRepository;
            this.vibeNetService = vibeNetService;
        }

        public async Task<IEnumerable<VibeNetUserProfileViewModel>?> GetFriendsAsync(string userId)
        {
            var entityList = await friendshipRepository.GetAllAttached()
                .Where(fs => fs.FirstUserId == userId || fs.SecondUserId == userId)
                .ToListAsync();

            IList<VibeNetUserProfileViewModel> friends = new List<VibeNetUserProfileViewModel>();

            foreach (var friend in entityList)
            {
                var friendId = string.Empty;

                if (friend.FirstUserId != userId)
                    friendId = friend.FirstUserId;
                else
                    friendId = friend.SecondUserId;

                VibeNetUserProfileViewModel? model = await vibeNetService.CreateVibeNetUserProfileViewModel(friendId);

                if (model != null)
                    friends.Add(model);
            }

            return friends;
        }

        public async Task<bool> FindByIdAsync(string firstUserId, string secondUserId)
        {
            var entityList = await friendshipRepository.GetAllAttached()
                .Where(fs => fs.FirstUserId == firstUserId && fs.SecondUserId == secondUserId ||
                fs.FirstUserId == secondUserId && fs.SecondUserId == firstUserId)
                .ToListAsync();

            if (entityList.Count > 0)
                return true;

            return false;
        }

        public async Task AddFriendShipAsync(Friendshiprequest entity)
        {

            Friendship friendship = new()
            {
                FirstUserId = entity.UserTransmitterId,
                SecondUserId = entity.UserRecipientId,
                FriendsSince = DateTime.Now,
            };

            await friendshipRepository.AddAsync(friendship);
        }

        public async Task DeleteAsync(string userId)
        {
            var friendShips = friendshipRepository.GetAllAttached()
                .Where(fs => fs.FirstUserId == userId || fs.SecondUserId == userId);

            if (friendShips == null) return;
            await friendshipRepository.DeleteEntityRangeAsync(friendShips);
        }
    }
}
