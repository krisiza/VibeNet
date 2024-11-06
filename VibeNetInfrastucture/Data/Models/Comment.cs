using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VibeNetInfrastucture.Validations;

namespace VibeNetInfrastucture.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int? PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public virtual Post? Post { get; set; }

        [Required]
        public virtual string OwnerId { get; set; } = null!;

        [ForeignKey(nameof(OwnerId))]
        public virtual IdentityUser Owner { get; set; } = null!;

        [MaxLength(ValidationConstants.Comment.ContentMaxLength)]
        [Required]
        public string Content { get; set; } = null!;

        public DateTime PostedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
