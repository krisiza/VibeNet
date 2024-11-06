using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeNetInfrastucture.Data.Models
{
    [PrimaryKey(nameof(UserRecipientId), nameof(UserTransmitterId))]
    public class Friendshiprequest
    {
        public string UserRecipientId { get; set; } = null!;

        [ForeignKey(nameof(UserRecipientId))]
        public virtual IdentityUser UserRecipient { get; set; } = null!;

        public string UserTransmitterId { get; set; } = null!;

        [ForeignKey(nameof(UserTransmitterId))]
        public virtual IdentityUser UserTransmitter { get; set; } = null!;

        [Required]
        public DateTime SendOn { get; set; }
    }
}
