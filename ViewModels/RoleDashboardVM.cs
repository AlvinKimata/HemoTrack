using HemoTrack.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using HemoTrack.ViewModels;

namespace HemoTrack.ViewModels
{
    public class RoleDashboardVM
    {
        public List<IdentityRole> Roles {get; set;}

        public List<UsersRoleViewModel> UsersRoleViewModels {get; set;}





        public List<User> Users { get; set; }
        public CreateRoleVM createRoleVM {get; set;}

        public ViewRoleVM viewRoleVM {get; set;}

        public RoleDashboardVM()
        {
            Roles = new List<IdentityRole>();
            // RoleInstances = new List<RoleInstanceVM>();
            createRoleVM = new CreateRoleVM();
            viewRoleVM = new ViewRoleVM(); 
        }
    }
}