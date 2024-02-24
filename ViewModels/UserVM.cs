using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HemoTrack.Models;

namespace HemoTrack.ViewModels
{
    public class UserVM
    {
        public User user {get; set;}
        public bool IsSelected {get; set;}
        public string UserName { get; set; }
    }
}
