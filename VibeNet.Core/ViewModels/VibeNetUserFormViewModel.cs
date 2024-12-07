using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using VibeNetInfrastucture.Data.Models.Enums;
using static VibeNetInfrastucture.Constants.Validations.DateTimeFormat;
using static VibeNetInfrastucture.Constants.Validations.User;

namespace VibeNet.Core.ViewModels
{
    public class VibeNetUserFormViewModel
    {
        public Guid Id { get; set; }

        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        [Required]
        public string FirstName { get; set; } = null!;

        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        [DisplayFormat(DataFormatString = Format, ApplyFormatInEditMode = true)]
        public string Birthday { get; set; } = null!;

        public string? CreatedOn { get; set; } 

        [StringLength(HomeTownMaxLength, MinimumLength = HomeTownMinLength)]
        [Required]
        public string? HomeTown { get; set; }

        public Gender? Gender { get; set; }

        public IFormFile? ProfilePictureFile { get; set; }
    }
}
