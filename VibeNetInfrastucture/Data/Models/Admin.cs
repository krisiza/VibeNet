using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Infrastucture.Data.Models
{
    [Comment("Application Admin")]
    public class Admin
    {
        [Key]
        [Comment("Admin identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("User Identifier")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;
    }
}
