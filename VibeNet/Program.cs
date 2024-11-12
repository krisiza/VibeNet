namespace VibeNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationDbContext(builder.Configuration);
            builder.Services.AddApplicationIdentity(builder.Configuration);

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddControllersWithViews();

            builder.Services.AddApplicationRepository();
            builder.Services.AddApplicationServices();

            var app = builder.Build();

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

            // Custom route for ShowProfile with username
            app.MapControllerRoute(
                name: "profile",
                pattern: "User/ShowProfile/{userId}",
                defaults: new { controller = "User", action = "ShowProfile" });

            app.MapControllerRoute(
                name: "profile",
                pattern: "Post/AllPosts/{userId}",
                defaults: new { controller = "Post", action = "AllPosts" });

            app.MapRazorPages();

            app.Run();

            //app.MapDefaultControllerRoute();
            app.MapRazorPages();

            app.Run();
        }

    
    }
}
