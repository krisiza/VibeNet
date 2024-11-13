using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using VibeNet.Infrastucture.Constants;
using VibeNetInfrastucture.Data.Models.Enums;
using static VibeNetInfrastucture.Constants.Validations.Post;

namespace VibeNet.Core.ViewModels
{
    public class VibeNetUserProfileViewModel
    {
        public int Id { get; set; }

        public string IdentityId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string Birthday { get; set; } = null!;

        public Gender? Gender { get; set; }

        public string HomeTown { get; set; } = null!;

        public ProfilePictureViewModel? ProfilePicture { get; set; } = null!;

        public IFormFile? ProfilePictureFile { get; set; }

        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = ErrorMessages.NeededLength)]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public string? PostContent { get; set; }

        public IEnumerable<PostViewModel>? Posts { get; set; }
    }
}
