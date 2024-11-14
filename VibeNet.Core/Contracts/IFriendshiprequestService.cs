namespace VibeNet.Core.Contracts
{
    public interface IFriendshiprequestService
    {
        Task SendRequestAsync(string userRecipient, string userTransmitter);

        Task<bool> FindByIdAsync(string userRecipient, string userTransmitter);
    }
}
