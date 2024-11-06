using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VibeNetInfrastucture.Validations;

namespace VibeNetInfrastucture.Data.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string OwnerId { get; set; } = null!;

        [ForeignKey(nameof(OwnerId))]
        public virtual IdentityUser Owner { get; set; } = null!;

        [MaxLength(ValidationConstants.Post.ContentMaxLength)]
        public string? Content { get; set; }

        public string? Picture { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }

        public IEnumerable<IdentityUser> UserLiked { get; set; } = new List<IdentityUser>();

        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        [Required]
        public bool IsDeleted { get; set; }
    }
}
