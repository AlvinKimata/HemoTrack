using System.Collections.Generic;
using HemoTrack.Models;
using HemoTrack.Data;
using System.ComponentModel.DataAnnotations;

namespace HemoTrack.ViewModels
{
    public class ViewRoleVM
    {
        public ViewRoleVM()
        {
            Users = new List<User>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        public List<User> Users { get; set; }
    }
}