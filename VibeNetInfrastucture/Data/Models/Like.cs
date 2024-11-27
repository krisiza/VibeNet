using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Infrastucture.Data.Models
{
    public class Like
    {
        [Comment("Like Identifier")]
        public int Id { get; set; }

        [Comment("Post Like Identifier")]
        public int? PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public virtual Post? Post { get; set; }

        [Required]
        [Comment("User Owner Identifier")]
        public virtual string OwnerId { get; set; } = null!;

        [ForeignKey(nameof(OwnerId))]
        public virtual IdentityUser Owner { get; set; } = null!;
    }
}
