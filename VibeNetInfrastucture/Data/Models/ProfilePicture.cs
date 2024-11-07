using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Infrastucture.Data.Models
{
    public class ProfilePicture
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual VibeNetUser? VibeNetUser { get; set; }

        public string? Name { get; set; } 

        public string? ContentType { get; set; }

        public byte[]? Data { get; set; }

        public bool? IsSelected { get; set; }
    }
}
