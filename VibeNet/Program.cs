using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VibeNet.Core.Mapping;
using VibeNet.Data;
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

            builder.Services.AddScoped<IRepository<VibeNetUser, int>, Repository<VibeNetUser, int>>();
            builder.Services.AddScoped<IRepository<Comment, int>, Repository<Comment, int>>();
            builder.Services.AddScoped<IRepository<Post, int>, Repository<Post, int>>();
            builder.Services.AddScoped<IRepository<Friendship, object>, Repository<Friendship, object>>();
            builder.Services.AddScoped<IRepository<Friendshiprequest, object>, Repository<Friendshiprequest, object>>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);

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
