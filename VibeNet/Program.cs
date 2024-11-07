using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.Services;
using VibeNet.Data;
using VibeNet.Infrastucture.Data.Models;
using VibeNet.Infrastucture.Repository;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNet.Models;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<VibeNetDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<VibeNetDbContext>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<IRepository<VibeNetUser, int>, BaseRepository<VibeNetUser, int>>();
            builder.Services.AddScoped<IRepository<Comment, int>, BaseRepository<Comment, int>>();
            builder.Services.AddScoped<IRepository<Post, int>, BaseRepository<Post, int>>();
            builder.Services.AddScoped<IRepository<Friendship, object>, BaseRepository<Friendship, object>>();
            builder.Services.AddScoped<IRepository<Friendshiprequest, object>, BaseRepository<Friendshiprequest, object>>();
            builder.Services.AddScoped<IRepository<IdentityUser, Guid>, BaseRepository<IdentityUser, Guid>>();
            builder.Services.AddScoped<IRepository<ProfilePicture, int>, BaseRepository<ProfilePicture, int>>();

            builder.Services.AddScoped<IVibeNetService, VibeNetService>();
            builder.Services.AddScoped<IIdentityUserService, IdentityUserService>();
            builder.Services.AddScoped<IProfilePictureService, ProfilePictureService>();

            builder.Services.AddControllersWithViews();

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
