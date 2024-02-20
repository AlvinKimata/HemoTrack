using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
// inject RoleManager<User> _roleManager;

namespace HemoTrack.ViewModels
{
    public class RoleDashboardVM
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
        public List<IdentityRole> RoleNames {get; set;}

        public CreateRoleVM createRoleVM {get; set;}

        public RoleDashboardVM()
        {
            createRoleVM = new CreateRoleVM();
        }
    }
}