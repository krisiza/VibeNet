using System.ComponentModel.DataAnnotations;
using static VibeNetInfrastucture.Constants.Validations.Comment;

namespace VibeNet.Core.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public virtual VibeNetUserProfileViewModel Owner { get; set; } = null!;

        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; set; } = null!;

        public DateTime PostedOn { get; set; }
    }
}
