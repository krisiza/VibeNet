using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static VibeNetInfrastucture.Constants.Validations.Comment;

namespace VibeNetInfrastucture.Data.Models
{
    [Comment("Post Comment From User")]
    public class Comment
    {
        [Comment("Comment Identifier")]
        public int Id { get; set; }

        [Comment("Post Comment Identifier")]
        public int? PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public virtual Post? Post { get; set; }

        [Required]
        [Comment("User Owner Identifier")]
        public virtual string OwnerId { get; set; } = null!;

        [ForeignKey(nameof(OwnerId))]
        public virtual IdentityUser Owner { get; set; } = null!;

        [MaxLength(ContentMaxLength)]
        [Required]
        [Comment("Comment Content")]
        public string Content { get; set; } = null!;

        [Comment("Comment Creation Date")]
        public DateTime PostedOn { get; set; }

        [Comment("Comment Is Active Or Not")]
        public bool IsDeleted { get; set; }
    }
}
