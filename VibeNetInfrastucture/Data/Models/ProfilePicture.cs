using Microsoft.EntityFrameworkCore;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Infrastucture.Data.Models
{
    [Comment("User Profil Picture")]
    public class ProfilePicture
    {
        [Comment("Picture Identifier")]
        public int Id { get; set; }

        [Comment("Picture Name")]
        public string? Name { get; set; }

        [Comment("Picture Type")]
        public string? ContentType { get; set; }

        [Comment("Picture Data")]
        public byte[]? Data { get; set; }

    }
}
