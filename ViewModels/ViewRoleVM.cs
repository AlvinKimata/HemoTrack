using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HemoTrack.ViewModels
{
    public class ViewRoleVM
    {
        public ViewRoleVM()
        {
            Users = new List<string>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}