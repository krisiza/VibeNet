﻿using Microsoft.AspNetCore.Http;
using VibeNetInfrastucture.Data.Models.Enums;

namespace VibeNet.Core.ViewModels
{
    public class VibeNetUserProfileViewModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string Birthday { get; set; } = null!;

        public Gender? Gender { get; set; }

        public string HomeTown { get; set; } = null!;

        public ProfilePictureViewModel ProfilePicture { get; set; } = null!;

        public IFormFile? ProfilePictureFile { get; set; }
    }
}