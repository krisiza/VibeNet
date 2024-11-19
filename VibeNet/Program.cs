using Microsoft.AspNetCore.Identity;
using VibeNet.Extensions;
using VibeNet.Infrastucture.Data;
using VibeNet.Infrastucture.SeedDb;
using Microsoft.EntityFrameworkCore;

namespace VibeNet
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("VibeNetDbContextConnection") ?? throw new InvalidOperationException("Connection string 'VibeNetDbContextConnection' not found.");

            builder.Services.AddApplicationDbContext(builder.Configuration);
            builder.Services.AddApplicationIdentity(builder.Configuration);
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages(); 
            builder.Services.AddApplicationRepository();
            builder.Services.AddApplicationServices();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                await app.Services.CreateScope().ServiceProvider.SeedDataAsync();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
              name: "default",
              pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "admin",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            // Custom route for ShowProfile with username
            app.MapControllerRoute(
                name: "profile",
                pattern: "User/ShowProfile/{userId}",
                defaults: new { controller = "User", action = "ShowProfile" });

            app.MapControllerRoute(
                name: "profile",
                pattern: "Post/AllPosts/{userId}",
                defaults: new { controller = "Post", action = "AllPosts" });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.MapRazorPages();

            app.Run();

            //app.MapDefaultControllerRoute();
            app.MapRazorPages();

            app.Run();
        }

    
    }
}
