using VibeNet.Core.ViewModels;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Core.Contracts
{
    public interface IFriendshipService
    {
        Task<IEnumerable<VibeNetUserProfileViewModel>?> GetFriendsAsync(string userId);
        Task<bool> FindByIdAsync(string firstUserId, string secondUserId);
        Task AddFriendShipAsync(Friendshiprequest entity);
        void Delete(string userId);
    }
}
