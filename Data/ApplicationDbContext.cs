using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HemoTrack.Models;

namespace HemoTrack.Data;

public class ApplicationDbContext :  IdentityDbContext<Patient>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

        public DbSet<Administrator>? Administrator {get; set;}
        public DbSet<Appointment>? Appointment {get; set;}
        public DbSet<Doctor>? Doctor {get; set;}
        public DbSet<Patient>? Patient { get; set; }
        public DbSet<Schedule>? Schedule {get; set;}
        public DbSet<Specialities>? Specialities { get; set; }
        public DbSet<Webuser>? Webuser {get; set;}

}
