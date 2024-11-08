using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeNetInfrastucture.Data.Models
{
    [Comment("Friendship between Users")]
    [PrimaryKey(nameof(FirstUserId), nameof(SecondUserId))]
    public class Friendship
    {
        [Comment("First User Identifier")]
        public string FirstUserId { get; set; } = null!;

        [ForeignKey(nameof(FirstUserId))]
        public virtual IdentityUser FirstUser { get; set; } = null!;

        [Comment("Second User Identifier")]
        public string SecondUserId { get; set; } = null!;

        [ForeignKey(nameof(SecondUserId))]
        public virtual IdentityUser SecondUser { get; set; } = null!;

        [Comment("Friendship Since Date")]
        public DateTime FriendsSince { get; set; }
    }
}
