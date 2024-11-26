using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VibeNet.Infrastucture.Data.Models;
using static VibeNetInfrastucture.Constants.Validations.Post;

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
        [MaxLength(ContentMaxLength)]
        public string? Content { get; set; }

        [Comment("Post Picture")]
        public int? Picture { get; set; }

        [Required]
        [Comment("Post Creation Date")]
        public DateTime PostedOn { get; set; }

        public IList<Like> UserLiked { get; set; } = new List<Like>();

        public IList<Comment> Comments { get; set; } = new List<Comment>();
    }
}
