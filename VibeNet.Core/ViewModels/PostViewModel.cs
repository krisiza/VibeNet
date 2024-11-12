using System.ComponentModel.DataAnnotations;
using VibeNetInfrastucture.Validations;

namespace VibeNet.Core.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public string OwnerId { get; set; } = null!;

        [StringLength(ValidationConstants.Post.ContentMaxLength, MinimumLength = ValidationConstants.Post.ContentMinLength)]
        public string? Content { get; set; }

        public string? Picture { get; set; }

        public DateTime PostedOn { get; set; }

        public IEnumerable<VibeNetUserProfileViewModel> UserLiked { get; set; } = new List<VibeNetUserProfileViewModel>();

        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

        public bool IsDeleted { get; set; }
    }
}
