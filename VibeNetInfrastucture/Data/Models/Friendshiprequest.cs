using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeNetInfrastucture.Data.Models
{
    [Comment("Friendshiprequest")]
    [PrimaryKey(nameof(UserRecipientId), nameof(UserTransmitterId))]
    public class Friendshiprequest
    {
        [Comment("Recipient Identifier")]
        public string UserRecipientId { get; set; } = null!;

        [ForeignKey(nameof(UserRecipientId))]
        public virtual IdentityUser UserRecipient { get; set; } = null!;

        [Comment("Transmitter Identifier")]
        public string UserTransmitterId { get; set; } = null!;

        [ForeignKey(nameof(UserTransmitterId))]
        public virtual IdentityUser UserTransmitter { get; set; } = null!;
    }
}
