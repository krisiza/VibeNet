using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeNetInfrastucture.Data.Models
{
    [PrimaryKey(nameof(FirstUserId), nameof(SecondUserId))]
    public class Friendship
    {
        public string FirstUserId { get; set; } = null!;

        [ForeignKey(nameof(FirstUserId))]
        public virtual IdentityUser FirstUser { get; set; } = null!;

        public string SecondUserId { get; set; } = null!;

        [ForeignKey(nameof(SecondUserId))]
        public virtual IdentityUser SecondUser { get; set; } = null!;

        public DateTime FriendsSince { get; set; }
    }
}
