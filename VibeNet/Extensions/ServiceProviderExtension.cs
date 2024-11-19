using Microsoft.AspNetCore.Identity;
using VibeNet.Infrastucture.Data;
using VibeNet.Infrastucture.SeedDb;

namespace VibeNet.Extensions
{
    public static class ServiceProviderExtension
    {
        public static async Task<IServiceProvider> SeedDataAsync(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                VibeNetDbContext context = services.GetRequiredService<VibeNetDbContext>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!context.Users.Any())
                {
                    await Seeder.SeedIdentityAsync(context, userManager);
                    await Seeder.SeedProfilePicturesAsync(context);
                    await Seeder.SeedVibenetUsers(context);
                    await Seeder.SeedPosts(context);
                    await Seeder.SeedComments(context);
                    await Seeder.SeedLikes(context);
                    await Seeder.SeedFriendshiprequest(context);
                    await Seeder.SeedFriendships(context);
                    await Seeder.SeedUserClaims(userManager);
                }
                    await Seeder.SeedManagerRole(roleManager, userManager);
            }

            return serviceProvider;
        }
    }
}
