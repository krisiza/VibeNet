using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeNetInfrastucture.Data.Models
{
    [PrimaryKey(nameof(UserRecipientId), nameof(UserTransmitterId))]
    public class Friendshiprequest
    {
        public required string UserRecipientId { get; set; }

        [ForeignKey(nameof(UserRecipientId))]
        public virtual IdentityUser UserRecipient { get; set; } = null!;

        public required string UserTransmitterId { get; set; }

        [ForeignKey(nameof(UserTransmitterId))]
        public virtual IdentityUser UserTransmitter { get; set; } = null!;

        public DateTime SendOn { get; set; }
    }
}
