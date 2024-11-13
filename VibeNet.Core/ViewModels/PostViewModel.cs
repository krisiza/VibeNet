using System.ComponentModel.DataAnnotations;
using VibeNet.Infrastucture.Constants;
using static VibeNetInfrastucture.Constants.Validations.Post;

namespace VibeNet.Core.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public string OwnerId { get; set; } = null!;

        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = ErrorMessages.NeededLength)]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public string Content { get; set; } = null!;

        public string? Picture { get; set; }

        public DateTime PostedOn { get; set; }

        public IEnumerable<VibeNetUserProfileViewModel> UserLiked { get; set; } = new List<VibeNetUserProfileViewModel>();

        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

        public bool IsDeleted { get; set; }
    }
}
