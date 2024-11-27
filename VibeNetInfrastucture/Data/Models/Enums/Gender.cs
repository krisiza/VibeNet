using System.ComponentModel.DataAnnotations;

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