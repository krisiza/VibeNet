namespace VibeNet.Core.ViewModels
{
    public class LikeViewModel
    {
        public int Id { get; set; }

        public virtual VibeNetUserProfileViewModel Owner { get; set; } = null!;

    }
}
