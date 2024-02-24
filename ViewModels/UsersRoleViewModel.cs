using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HemoTrack.ViewModels
{
    public class UsersRoleViewModel
    {
        public IdentityRole Role {get; set;}
        public List<UserVM> UsersInRole {get; set;}
    }
}
