//Role Instance.

using HemoTrack.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HemoTrack.ViewModels
{
    public class RoleInstanceVM
    {
        public string RoleName {get; set;}

        public List<User> UsersInRole {get; set;}

        public IdentityRole Role {get; set;}

    }
}