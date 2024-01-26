using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HemoTrack.Models;
using Microsoft.AspNetCore.Identity;

namespace HemoTrack.Data;

public class ApplicationDbContext :  IdentityDbContext<Patient>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

        public DbSet<Appointment> Appointment {get; set;}
        public DbSet<Doctor> Doctor {get; set;}
        public DbSet<Administrator> Administrator {get; set;}
        public DbSet<Schedule> Schedule {get; set;}
        public DbSet<Patient> Patient {get; set;}
        public DbSet<Specialities> Specialities { get; set; }
        public DbSet<Webuser> Webuser {get; set;}
        public DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Administrator>();
        builder.Entity<Patient>();
        builder.Entity<IdentityUserLogin<string>>().HasKey(e => new { e.LoginProvider, e.ProviderKey });
        builder.Entity<IdentityUserRole<string>>().HasKey(e => new { e.UserId, e.RoleId});
        builder.Entity<IdentityUserToken<string>>().HasKey(e => new {e.UserId, e.LoginProvider, e.Name});
    }

}
