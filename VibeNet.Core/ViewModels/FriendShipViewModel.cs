namespace VibeNet.Core.ViewModels
{
    public class FriendShipViewModel
    {
        public virtual VibeNetUserProfileViewModel FirstUser { get; set; } = null!;

        public virtual VibeNetUserProfileViewModel SecondUser { get; set; } = null!;

        public DateTime FriendsSince { get; set; }
    }
}
