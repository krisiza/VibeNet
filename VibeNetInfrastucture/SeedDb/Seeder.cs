using Microsoft.AspNetCore.Identity;
using VibeNet.Infrastucture.Data;
using VibeNet.Infrastucture.Data.Models;
using VibeNet.Infrastucture.Utilities;

namespace VibeNet.Infrastucture.SeedDb
{
    public static class Seeder
    {
        public static IdentityUser[] IdentityUsers = new IdentityUser[10];
        public static ProfilePicture[] ProfilePictures = new ProfilePicture[10];

        public static async Task SeedIdentityAsync(VibeNetDbContext context, UserManager<IdentityUser> userManager)
        {

            IdentityUsers[0] = new IdentityUser
            {
                UserName = "jane@example.com",
                Email = "jane@example.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(IdentityUsers[0], "Password123!");

            IdentityUsers[1] = new IdentityUser
            {
                UserName = "tom@example.com",
                Email = "tom@example.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(IdentityUsers[1], "Password123!");

            IdentityUsers[2] = new IdentityUser
            {
                UserName = "niko@example.com",
                Email = "niko@example.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(IdentityUsers[2], "Password123!");

            IdentityUsers[3] = new IdentityUser
            {
                UserName = "alexandra@example.com",
                Email = "alexandra@example.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(IdentityUsers[3], "Password123!");

            IdentityUsers[4] = new IdentityUser
            {
                UserName = "daniel@example.com",
                Email = "daniel@example.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(IdentityUsers[4], "Password123!");

            IdentityUsers[5] = new IdentityUser
            {
                UserName = "emily@example.com",
                Email = "emily@example.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(IdentityUsers[5], "Password123!");

            IdentityUsers[6] = new IdentityUser
            {
                UserName = "john@example.com",
                Email = "john@example.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(IdentityUsers[6], "Password123!");

            IdentityUsers[7] = new IdentityUser
            {
                UserName = "michael@example.com",
                Email = "michael@example.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(IdentityUsers[7], "Password123!");


            IdentityUsers[8] = new IdentityUser
            {
                UserName = "sarah@example.com",
                Email = "sarah@example.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(IdentityUsers[8], "Password123!");


            IdentityUsers[9] = new IdentityUser
            {
                UserName = "claire@example.com",
                Email = "claire@example.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(IdentityUsers[9], "Password123!");

            await context.SaveChangesAsync();
        }

        public static async Task SeedProfilePicturesAsync(VibeNetDbContext context)
        {
            ProfilePictures[0] = new ProfilePicture
            {
                Name = "jane",
                ContentType = "jpeg",
                Data = await PictureHelper.ConvertToBytesAsync("jane.jpeg")
            };

            ProfilePictures[1] = new ProfilePicture
            {
                Name = "tom",
                ContentType = "jpeg",
                Data = await PictureHelper.ConvertToBytesAsync("tom.jpeg")
            };

            ProfilePictures[2] = new ProfilePicture
            {
                Name = "niko",
                ContentType = "jpeg",
                Data = await PictureHelper.ConvertToBytesAsync("niko.jpeg")
            };

            ProfilePictures[3] = new ProfilePicture
            {
                Name = "alexandra",
                ContentType = "jpeg",
                Data = await PictureHelper.ConvertToBytesAsync("alexandra.jpeg")
            };

            ProfilePictures[4] = new ProfilePicture
            {
                Name = "daniel",
                ContentType = "jpg",
                Data = await PictureHelper.ConvertToBytesAsync("daniel.jpg")
            };

            ProfilePictures[5] = new ProfilePicture
            {
                Name = "emily",
                ContentType = "jpeg",
                Data = await PictureHelper.ConvertToBytesAsync("emily.jpeg")
            };

            ProfilePictures[6] = new ProfilePicture
            {
                Name = "john",
                ContentType = "jpeg",
                Data = await PictureHelper.ConvertToBytesAsync("john.jpeg")
            };


            ProfilePictures[7] = new ProfilePicture
            {
                Name = "michael",
                ContentType = "jpeg",
                Data = await PictureHelper.ConvertToBytesAsync("michael.jpeg")
            };

            ProfilePictures[8] = new ProfilePicture
            {
                Name = "sarah",
                ContentType = "jpeg",
                Data = await PictureHelper.ConvertToBytesAsync("sarah.jpeg")
            };

            ProfilePictures[9] = new ProfilePicture
            {
                Name = "claire",
                ContentType = "jpg",
                Data = await PictureHelper.ConvertToBytesAsync("claire.jpg")
            };

            await context.ProfilePictures.AddRangeAsync(ProfilePictures);
            await context.SaveChangesAsync();
        }
    }
}

