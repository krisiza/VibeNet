using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeNetInfrastucture.Data.Models.Enums
{
    public enum Gender
    {
        [Display(Name = "Male")]
        Male = 0,
        [Display(Name = "Female")]
        Female = 1,
        [Display(Name = "Diverse")]
        Diverse = 2
    }
}