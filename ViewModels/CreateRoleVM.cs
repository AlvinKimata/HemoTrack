using System.ComponentModel.DataAnnotations;

namespace HemoTrack.ViewModels
{
    public class CreateRoleVM
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}