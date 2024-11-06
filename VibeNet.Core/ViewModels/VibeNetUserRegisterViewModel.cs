﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using VibeNetInfrastucture.Data.Models;
using VibeNetInfrastucture.Data.Models.Enums;
using VibeNet.Core.Mapping;
using static VibeNetInfrastucture.Validations.ValidationConstants.User;

namespace VibeNet.Core.ViewModels
{
    public class VibeNetUserRegisterViewModel : IMapFrom<VibeNetUser>
    {
        public Guid Id { get; set; }

        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        [Required]
        public string FirstName { get; set; } = null!;

        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        [Required]
        public string Lastname { get; set; } = null!;

        [Required]
        public string Birthday { get; set; } = null!;

        public string CreatedOn { get; set; } = null!;

        [StringLength(HomeTownMaxLength, MinimumLength = HomeTownMinLength)]
        [Required]
        public string? HomeTown { get; set; }

        public Gender? Gender { get; set; }

        public IFormFile? ProfilePicture { get; set; }
    }
}
