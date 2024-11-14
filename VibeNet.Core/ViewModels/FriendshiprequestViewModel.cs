namespace VibeNet.Core.ViewModels
{
    public class FriendshiprequestViewModel
    {
        public virtual VibeNetUserProfileViewModel UserRecipient { get; set; } = null!;

        public virtual VibeNetUserProfileViewModel UserTransmitter { get; set; } = null!;
    }
}
