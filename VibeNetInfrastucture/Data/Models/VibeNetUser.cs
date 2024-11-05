using System.ComponentModel.DataAnnotations;
using VibeNetInfrastucture.Data.Models.Enums;
using static VibeNetInfrastucture.Validations.ValidationConstants.User;

namespace VibeNetInfrastucture.Data.Models
{
    public class VibeNetUser
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(LastNameMaxLength)]
        public required string Lastname { get; set; }

        public DateTime Birthday { get; set; }

        public DateTime CreatedOn { get; set; }

        [MaxLength(HomeTownMaxLength)]
        public string? HomeTown { get; set; }

        public Gender? Gender { get; set; }

        public string? ProfilePicture { get; set; }

        public bool IsDeleted { get; set; }
    }
}
