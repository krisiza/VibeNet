﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VibeNetInfrastucture.Data.Models.Enums;
using static VibeNetInfrastucture.Validations.ValidationConstants.User;

namespace VibeNetInfrastucture.Data.Models
{
    public class VibeNetUser
    {
        public int Id { get; set; }

        public string VibeNetUserId { get; set; } = null!;

        [ForeignKey(nameof(VibeNetUserId))]
        public virtual IdentityUser User { get; set; } = null!;

        [MaxLength(FirstNameMaxLength)]
        public required string FirstName { get; set; } 

        [MaxLength(LastNameMaxLength)]
        public required string Lastname { get; set; } 

        public required DateTime Birthday { get; set; }

        public required DateTime CreatedOn { get; set; }

        [MaxLength(HomeTownMaxLength)]
        public string? HomeTown { get; set; }

        public Gender? Gender { get; set; }

        public string? ProfilePicture { get; set; }

        public bool IsDeleted { get; set; }
    }
}