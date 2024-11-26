using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VibeNet.Infrastucture.Data.Models;
using VibeNetInfrastucture.Data.Models.Enums;
using static VibeNetInfrastucture.Constants.Validations.User;

namespace VibeNetInfrastucture.Data.Models
{
    [Comment("Application User")]
    public class VibeNetUser
    {
        [Key]
        [Comment("User Identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Identity User Identifier")]
        public string IdentityUserId { get; set; } = null!;

        [ForeignKey(nameof(IdentityUserId))]
        public virtual IdentityUser? User { get; set; }

        [MaxLength(FirstNameMaxLength)]
        [Required]
        [Comment("User Firstname")]
        public string FirstName { get; set; } = null!;

        [MaxLength(LastNameMaxLength)]
        [Required]
        [Comment("User Secondname")]
        public string LastName { get; set; } = null!;

        [Required]
        [Comment("User Birthday")]
        public DateTime Birthday { get; set; }

        [Required]
        [Comment("User Profile Creation Date")]
        public DateTime CreatedOn { get; set; }

        [MaxLength(HomeTownMaxLength)]
        [Comment("User HomeTown")]
        public string? HomeTown { get; set; }

        [Comment("User Gender")]
        public Gender? Gender { get; set; }

        [Comment("User ProfilPicture Identifier")]
        public int ProfilePictureId { get; set; }

        [ForeignKey(nameof(ProfilePictureId))]
        public virtual ProfilePicture? ProfilePicture { get; set; }
    }
}
