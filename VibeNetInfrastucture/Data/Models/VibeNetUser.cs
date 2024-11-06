using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using VibeNetInfrastucture.Data.Models.Enums;
using static VibeNetInfrastucture.Validations.ValidationConstants.User;

namespace VibeNetInfrastucture.Data.Models
{
    public class VibeNetUser
    {
        public int Id { get; set; }

        [Required]
        public string VibeNetUserId { get; set; } = null!;

        [ForeignKey(nameof(VibeNetUserId))]
        public virtual IdentityUser User { get; set; } = null!;

        [MaxLength(FirstNameMaxLength)]
        [Required]
        public string FirstName { get; set; } = null!;

        [MaxLength(LastNameMaxLength)]
        [Required]
        public string Lastname { get; set; } = null!;
        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [MaxLength(HomeTownMaxLength)]
        public string? HomeTown { get; set; }

        public Gender? Gender { get; set; }

        public byte[] ProfilePicture { get; set; }

        [Required]  
        public bool IsDeleted { get; set; }
    }
}
