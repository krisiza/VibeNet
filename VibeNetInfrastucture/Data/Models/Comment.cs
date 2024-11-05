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

        public required virtual string OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual IdentityUser Owner { get; set; } = null!;

        [MaxLength(ValidationConstants.Comment.ContentMaxLength)]
        public required string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
