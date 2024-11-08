using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeNetInfrastucture.Validations;

namespace VibeNet.Core.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public virtual PostViewModel Post { get; set; }

        public virtual string OwnerId { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime PostedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
