using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HemoTrack.Data;
using HemoTrack.Models;
using HemoTrack.Services;

using Quartz;
using HemoTrack.AspNetCoreQuartz;
using HemoTrack.AspNetCoreQuartz.QuartzServices;


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<BlogStoreDatabaseSettings>(
    builder.Configuration.GetSection("BlogsStoreDataBase"));

builder.Services.AddSingleton<BlogsService>();

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null
    );
    
builder.Services.AddSignalR();
builder.Services.AddQuartz(
    q => {
        var conconcurrentJobKey = new JobKey("ConconcurrentJob");
        q.AddJob<AppointmentJob>(opts => opts.WithIdentity(conconcurrentJobKey));
        q.AddTrigger(opts => opts.ForJob(conconcurrentJobKey)
            .ForJob(conconcurrentJobKey)
            .WithIdentity("ConconcurrentJob-trigger")
            .WithSimpleSchedule( x => x.WithIntervalInSeconds(5).RepeatForever()));
        
    });

builder.Services.AddQuartzHostedService(
    q => q.WaitForJobsToComplete = true
);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("HemoTrackDbConnection");
builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddSignInManager<SignInManager<User>>();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseEndpoints
(
    endpoints => {endpoints.MapHub<JobsHub>("/jobshub");
});

app.Run();