using System.ComponentModel.DataAnnotations;
using VibeNet.Infrastucture.Constants;
using VibeNetInfrastucture.Data.Models;
using static VibeNetInfrastucture.Constants.Validations.Post;

namespace VibeNet.Core.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public string OwnerId { get; set; } = null!;

        public VibeNetUserProfileViewModel Owner { get; set; } = null!;

        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = ErrorMessages.NeededLength)]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public string Content { get; set; } = null!;

        public int? Picture { get; set; }

        public DateTime PostedOn { get; set; }

        public IList<LikeViewModel> UserLiked { get; set; } = new List<LikeViewModel>();

        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}
