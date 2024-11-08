namespace VibeNet.Core.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public string OwnerIdentityId { get; set; } = null!;

        public string? Content { get; set; }

        public string? Picture { get; set; }

        public DateTime PostedOn { get; set; }

        public IEnumerable<VibeNetUserProfileViewModel> UserLiked { get; set; } = new List<VibeNetUserProfileViewModel>();

        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

        public bool IsDeleted { get; set; }
    }
}
