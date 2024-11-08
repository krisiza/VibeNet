using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.Services;
using VibeNet.Data;
using VibeNet.Infrastucture.Data.Models;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNet.Infrastucture.Repository;
using VibeNetInfrastucture.Data.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IVibeNetService, VibeNetService>();
            services.AddScoped<IIdentityUserService, IdentityUserService>();
            services.AddScoped<IProfilePictureService, ProfilePictureService>();
            services.AddScoped<IPostService, PostService>();

            return services;
        }

        public static IServiceCollection AddApplicationRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepository<VibeNetUser, int>, BaseRepository<VibeNetUser, int>>();
            services.AddScoped<IRepository<Comment, int>, BaseRepository<Comment, int>>();
            services.AddScoped<IRepository<Post, int>, BaseRepository<Post, int>>();
            services.AddScoped<IRepository<Friendship, object>, BaseRepository<Friendship, object>>();
            services.AddScoped<IRepository<Friendshiprequest, object>, BaseRepository<Friendshiprequest, object>>();
            services.AddScoped<IRepository<IdentityUser, Guid>, BaseRepository<IdentityUser, Guid>>();
            services.AddScoped<IRepository<ProfilePicture, int>, BaseRepository<ProfilePicture, int>>();
            services.AddScoped<IRepository<Post, int>, BaseRepository<Post, int>>();
            services.AddScoped<IRepository<Comment, int>, BaseRepository<Comment, int>>();

            return services;
        }

        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<VibeNetDbContext>(options =>
                 options.UseSqlServer(connectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }

        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                     .AddEntityFrameworkStores<VibeNetDbContext>();

            return services;
        }
    }
}
