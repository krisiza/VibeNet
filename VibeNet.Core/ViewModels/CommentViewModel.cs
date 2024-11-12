using System.ComponentModel.DataAnnotations;
using VibeNetInfrastucture.Validations;

namespace VibeNet.Core.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public virtual VibeNetUserProfileViewModel Owner { get; set; } = null!;

        [StringLength(ValidationConstants.Comment.ContentMaxLength, MinimumLength = ValidationConstants.Comment.ContentMinLength)]
        public string Content { get; set; } = null!;

        public DateTime PostedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
