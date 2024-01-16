using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HemoTrack.Models;

namespace HemoTrack.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
        public DbSet<Patients> Patients { get; set; }
        public DbSet<Doctors> Doctors { get; set; }
        public DbSet<Specialities> Specialities { get; set; }
}
