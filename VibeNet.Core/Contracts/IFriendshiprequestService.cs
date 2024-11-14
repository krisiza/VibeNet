using VibeNet.Core.ViewModels;

namespace VibeNet.Core.Contracts
{
    public interface IFriendshiprequestService
    {
        Task<IEnumerable<VibeNetUserProfileViewModel>?> GetFriendrequets(string userId);

        Task SendRequestAsync(string userRecipient, string userTransmitter);

        Task<bool> FindByIdAsync(string userRecipient, string userTransmitter);
    }
}
