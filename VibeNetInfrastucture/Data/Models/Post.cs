using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VibeNetInfrastucture.Validations;

namespace VibeNetInfrastucture.Data.Models
{
    [Comment("User Post")]
    public class Post
    {
        [Comment("Post Identifier")]
        public int Id { get; set; }

        [Comment("Post Owner Identifier")]
        public string OwnerId { get; set; } = null!;

        [ForeignKey(nameof(OwnerId))]
        public virtual IdentityUser Owner { get; set; } = null!;

        [Comment("Post Content")]
        [MaxLength(ValidationConstants.Post.ContentMaxLength)]
        public string? Content { get; set; }

        [Comment("Post Picture")]
        public string? Picture { get; set; }

        [Required]
        [Comment("Post Creation Date")]
        public DateTime PostedOn { get; set; }

        public IList<IdentityUser> UserLiked { get; set; } = new List<IdentityUser>();

        public IList<Comment> Comments { get; set; } = new List<Comment>();

        [Required]
        [Comment("Post Is Active Or Not")]
        public bool IsDeleted { get; set; }
    }
}
