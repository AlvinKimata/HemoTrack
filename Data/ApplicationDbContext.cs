using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HemoTrack.Models;
using Microsoft.AspNetCore.Identity;
using MySqlConnector;

namespace HemoTrack.Data;

public class ApplicationDbContext :  IdentityDbContext<User>
{
    public async Task<MySqlConnection> OpenConnectionAsync()
    {
        var connectionString = "server=localhost;user id=root; password=; Database=HemoTrack;";
        var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        return connection;
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

        public DbSet<Appointment> Appointment {get; set;}
        public DbSet<Doctor> Doctor {get; set;}
        public DbSet<Administrator> Administrator {get; set;}
        public DbSet<Patient> Patient {get; set;}
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
