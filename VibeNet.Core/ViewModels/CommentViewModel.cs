namespace VibeNet.Core.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public virtual VibeNetUserProfileViewModel Owner { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime PostedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
