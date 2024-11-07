using System.ComponentModel.DataAnnotations.Schema;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Core.ViewModels
{
    public class ProfilePictureViewModel
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        public string? Name { get; set; }

        public string? ContentType { get; set; }

        public byte[]? Data { get; set; }

        public bool? IsSelected { get; set; }
    }
}
