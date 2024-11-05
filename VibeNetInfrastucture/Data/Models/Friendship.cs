using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeNetInfrastucture.Data.Models
{
    [PrimaryKey(nameof(FirstUserId), nameof(SecondUserId))]
    public class Friendship
    {
        public required string FirstUserId { get; set; }

        [ForeignKey(nameof(FirstUserId))]
        public virtual IdentityUser FirstUser { get; set; } = null!;

        public required string SecondUserId { get; set; }

        [ForeignKey(nameof(SecondUserId))]
        public virtual IdentityUser SecondUser { get; set; } = null!;

        public DateTime FriendsSince { get; set; }
    }
}
