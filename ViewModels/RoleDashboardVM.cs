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
        public string RoleName {get; set;}

        public List<IdentityRole> RoleNames {get; set;}

        public List<RoleInstanceVM> RoleInstances {get; set;}

        public CreateRoleVM createRoleVM {get; set;}

        public ViewRoleVM viewRoleVM {get; set;}

        public RoleDashboardVM()
        {
            RoleNames = new List<IdentityRole>();
            RoleInstances = new List<RoleInstanceVM>();
            createRoleVM = new CreateRoleVM();
            viewRoleVM = new ViewRoleVM(); 
        }
    }
}