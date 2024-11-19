using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VibeNet.Infrastucture.Data;
using VibeNet.Infrastucture.Data.Models;
using VibeNet.Infrastucture.Utilities;
using VibeNetInfrastucture.Data.Models;
using VibeNetInfrastucture.Data.Models.Enums;
using static VibeNet.Infrastucture.Constants.CustomClaims;
using static VibeNet.Infrastucture.Constants.AdminConstant;
using VibeNet.Infrastucture.Constants;

namespace VibeNet.Infrastucture.SeedDb
{
    public static class Seeder
    {
        public static IdentityUser[] IdentityUsers = new IdentityUser[10];
        public static ProfilePicture[] ProfilePictures = new ProfilePicture[10];
        public static VibeNetUser[] VibeNetUsers = new VibeNetUser[10];
        public static IdentityUserClaim<string>[] UserClaims = new IdentityUserClaim<string>[10];
        public static Post[] Posts = new Post[50];
        public static Comment[] Comments = new Comment[100];
        public static Like[] Likes = new Like[500];
        public static Friendshiprequest[] FriendshipRequests = new Friendshiprequest[10];
        public static List<Friendship> Friendships = new List<Friendship>();

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

        public static async Task SeedVibenetUsers(VibeNetDbContext context)
        {
            VibeNetUsers[0] = new VibeNetUser()
            {
                User = IdentityUsers[0],
                FirstName = "Jane",
                LastName = "Smith",
                Birthday = DateTime.Parse("1996-04-15"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "Springfield",
                Gender = Gender.Female,
                ProfilePictureId = ProfilePictures[0].Id,
                IsDeleted = false,
            };

            VibeNetUsers[1] = new VibeNetUser()
            {
                User = IdentityUsers[1],
                FirstName = "Tom",
                LastName = "Garcia",
                Birthday = DateTime.Parse("1993-11-01"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "Sydney",
                Gender = Gender.Male,
                ProfilePictureId = ProfilePictures[1].Id,
                IsDeleted = false
            };

            VibeNetUsers[2] = new VibeNetUser()
            {
                User = IdentityUsers[2],
                FirstName = "Nikolai",
                LastName = "Werner",
                Birthday = DateTime.Parse("1997-11-20"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "Emhof",
                Gender = Gender.Male,
                ProfilePictureId = ProfilePictures[2].Id,
                IsDeleted = false,
            };

            VibeNetUsers[3] = new VibeNetUser()
            {
                User = IdentityUsers[3],
                FirstName = "Alexandra",
                LastName = "Schmidt",
                Birthday = DateTime.Parse("2000-02-06"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "Cape Town",
                Gender = Gender.Diverse,
                ProfilePictureId = ProfilePictures[3].Id,
                IsDeleted = false,
            };

            VibeNetUsers[4] = new VibeNetUser()
            {
                User = IdentityUsers[4],
                FirstName = "Daniel",
                LastName = "Murphy",
                Birthday = DateTime.Parse("1988-07-30"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "Patel",
                Gender = Gender.Male,
                ProfilePictureId = ProfilePictures[3].Id,
                IsDeleted = false,
            };

            VibeNetUsers[5] = new VibeNetUser()
            {
                User = IdentityUsers[5],
                FirstName = "Emily",
                LastName = "Nguyen",
                Birthday = DateTime.Parse("1999-01-30"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "Tokyo",
                Gender = Gender.Female,
                ProfilePictureId = ProfilePictures[3].Id,
                IsDeleted = false,
            };

            VibeNetUsers[6] = new VibeNetUser()
            {
                User = IdentityUsers[6],
                FirstName = "John",
                LastName = "Ivanov",
                Birthday = DateTime.Parse("1994-12-29"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "Sofia",
                Gender = Gender.Male,
                ProfilePictureId = ProfilePictures[6].Id,
                IsDeleted = false,
            };

            VibeNetUsers[7] = new VibeNetUser()
            {
                User = IdentityUsers[7],
                FirstName = "Michael",
                LastName = "Hernández",
                Birthday = DateTime.Parse("1995-10-14"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "Toronto",
                Gender = Gender.Male,
                ProfilePictureId = ProfilePictures[7].Id,
                IsDeleted = false,
            };

            VibeNetUsers[8] = new VibeNetUser()
            {
                User = IdentityUsers[8],
                FirstName = "Sahra",
                LastName = "Kim",
                Birthday = DateTime.Parse("2001-06-11"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "São Paulo",
                Gender = Gender.Female,
                ProfilePictureId = ProfilePictures[7].Id,
                IsDeleted = false,
            };

            VibeNetUsers[9] = new VibeNetUser()
            {
                User = IdentityUsers[9],
                FirstName = "Claire",
                LastName = "Lopez",
                Birthday = DateTime.Parse("1992-09-22"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "Moscow",
                Gender = Gender.Female,
                ProfilePictureId = ProfilePictures[7].Id,
                IsDeleted = false,
            };

            await context.VibeNetUsers.AddRangeAsync(VibeNetUsers);
            await context.SaveChangesAsync();
        }

        public static async Task SeedPosts(VibeNetDbContext context)
        {
            Posts[0] = new Post
            {
                OwnerId = IdentityUsers[0].Id,
                Content = "Excited to share my journey in Springfield!",
                PostedOn = DateTime.Now.AddDays(-10),
                IsDeleted = false
            };
            Posts[1] = new Post
            {
                OwnerId = IdentityUsers[2].Id,
                Content = "Life in Emhof is peaceful and inspiring.",
                PostedOn = DateTime.Now.AddDays(-15),
                IsDeleted = false
            };
            Posts[2] = new Post
            {
                OwnerId = IdentityUsers[3].Id,
                Content = "Cape Town sunsets are breathtaking!",
                PostedOn = DateTime.Now.AddDays(-20),
                IsDeleted = false
            };
            Posts[3] = new Post
            {
                OwnerId = IdentityUsers[4].Id,
                Content = "Sharing thoughts on life in Patel.",
                PostedOn = DateTime.Now.AddDays(-5),
                IsDeleted = false
            };
            Posts[4] = new Post
            {
                OwnerId = IdentityUsers[5].Id,
                Content = "Tokyo's energy is unbeatable!",
                PostedOn = DateTime.Now.AddDays(-30),
                IsDeleted = false
            };
            Posts[5] = new Post
            {
                OwnerId = IdentityUsers[5].Id,
                Content = "Tokyo's energy is unbeatable!",
                PostedOn = DateTime.Now.AddDays(-30),
                IsDeleted = false
            };

            Posts[6] = new Post
            {
                OwnerId = IdentityUsers[6].Id,
                Content = "Sofia feels like home.",
                PostedOn = DateTime.Now.AddDays(-25),
                IsDeleted = false
            };

            Posts[7] = new Post
            {
                OwnerId = IdentityUsers[7].Id,
                Content = "Toronto's winter is magical.",
                PostedOn = DateTime.Now.AddDays(-8),
                IsDeleted = false
            };

            Posts[8] = new Post
            {
                OwnerId = IdentityUsers[8].Id,
                Content = "São Paulo's nightlife is amazing!",
                PostedOn = DateTime.Now.AddDays(-18),
                IsDeleted = false
            };

            Posts[9] = new Post
            {
                OwnerId = IdentityUsers[9].Id,
                Content = "Exploring Moscow's architecture.",
                PostedOn = DateTime.Now.AddDays(-22),
                IsDeleted = false
            };

            for (int i = 10; i < 50; i++)
            {
                Posts[i] = new Post
                {
                    OwnerId = IdentityUsers[i % IdentityUsers.Length].Id,
                    Content = $"Random thoughts post #{i} from user {i % IdentityUsers.Length}.",
                    PostedOn = DateTime.Now.AddDays(-i),
                    IsDeleted = false
                };
            }

            await context.Posts.AddRangeAsync(Posts);
            await context.SaveChangesAsync();
        }

        public static async Task SeedComments(VibeNetDbContext context)
        {
            Comments[0] = new Comment()
            {
                OwnerId = IdentityUsers[0].Id,
                PostId = Posts[0].Id,
                Content = "I wish you a lot of great moments on your journey!",
                PostedOn = DateTime.Now.AddDays(-10),
                IsDeleted = false
            };

            Comments[1] = new Comment()
            {
                OwnerId = IdentityUsers[1].Id,
                PostId = Posts[1].Id,
                Content = "This is amazing news, congrats!",
                PostedOn = DateTime.Now.AddDays(-9),
                IsDeleted = false
            };

            Comments[2] = new Comment()
            {
                OwnerId = IdentityUsers[2].Id,
                PostId = Posts[2].Id,
                Content = "Looking forward to seeing more updates!",
                PostedOn = DateTime.Now.AddDays(-8),
                IsDeleted = false
            };

            Comments[3] = new Comment()
            {
                OwnerId = IdentityUsers[3].Id,
                PostId = Posts[3].Id,
                Content = "Such a fantastic achievement, keep it up!",
                PostedOn = DateTime.Now.AddDays(-7),
                IsDeleted = false
            };

            Comments[4] = new Comment()
            {
                OwnerId = IdentityUsers[4].Id,
                PostId = Posts[4].Id,
                Content = "So proud of you for this accomplishment!",
                PostedOn = DateTime.Now.AddDays(-6),
                IsDeleted = false
            };

            Comments[5] = new Comment()
            {
                OwnerId = IdentityUsers[5].Id,
                PostId = Posts[5].Id,
                Content = "What a wonderful experience to share!",
                PostedOn = DateTime.Now.AddDays(-5),
                IsDeleted = false
            };

            Comments[6] = new Comment()
            {
                OwnerId = IdentityUsers[6].Id,
                PostId = Posts[6].Id,
                Content = "This looks incredible, thanks for sharing!",
                PostedOn = DateTime.Now.AddDays(-4),
                IsDeleted = false
            };

            Comments[7] = new Comment()
            {
                OwnerId = IdentityUsers[7].Id,
                PostId = Posts[7].Id,
                Content = "You’re doing an amazing job!",
                PostedOn = DateTime.Now.AddDays(-3),
                IsDeleted = false
            };

            Comments[8] = new Comment()
            {
                OwnerId = IdentityUsers[8].Id,
                PostId = Posts[8].Id,
                Content = "Thanks for the inspiration, keep shining!",
                PostedOn = DateTime.Now.AddDays(-2),
                IsDeleted = false
            };

            Comments[9] = new Comment()
            {
                OwnerId = IdentityUsers[9].Id,
                PostId = Posts[9].Id,
                Content = "A true testament to your dedication!",
                PostedOn = DateTime.Now.AddDays(-1),
                IsDeleted = false
            };

            for (int i = 10; i < 100; i++)
            {
                Comments[i] = new Comment()
                {
                    OwnerId = IdentityUsers[i % IdentityUsers.Length].Id,
                    PostId = Posts[i % Posts.Length].Id,
                    Content = $"This is a comment #{i} on post #{i % Posts.Length}.",
                    PostedOn = DateTime.Now.AddDays(-i),
                    IsDeleted = false
                };
            }

            await context.Comments.AddRangeAsync(Comments);
            await context.SaveChangesAsync();
        }

        public static async Task SeedLikes(VibeNetDbContext context)
        {
            for (int i = 0; i < 500; i++)
            {
                Likes[i] = new Like()
                {
                    PostId = Posts[i % Posts.Length].Id,
                    OwnerId = IdentityUsers[i % IdentityUsers.Length].Id,
                };
            }

            await context.AddRangeAsync(Likes);
            await context.SaveChangesAsync();
        }

        public static async Task SeedFriendshiprequest(VibeNetDbContext context)
        {
            FriendshipRequests[0] = new Friendshiprequest()
            {
                UserRecipientId = IdentityUsers[0].Id,
                UserTransmitterId = IdentityUsers[1].Id,
            };

            FriendshipRequests[1] = new Friendshiprequest()
            {
                UserRecipientId = IdentityUsers[1].Id,
                UserTransmitterId = IdentityUsers[2].Id,
            };

            FriendshipRequests[2] = new Friendshiprequest()
            {
                UserRecipientId = IdentityUsers[2].Id,
                UserTransmitterId = IdentityUsers[3].Id,
            };

            FriendshipRequests[3] = new Friendshiprequest()
            {
                UserRecipientId = IdentityUsers[3].Id,
                UserTransmitterId = IdentityUsers[4].Id,
            };

            FriendshipRequests[4] = new Friendshiprequest()
            {
                UserRecipientId = IdentityUsers[4].Id,
                UserTransmitterId = IdentityUsers[5].Id,
            };

            FriendshipRequests[5] = new Friendshiprequest()
            {
                UserRecipientId = IdentityUsers[5].Id,
                UserTransmitterId = IdentityUsers[6].Id,
            };

            FriendshipRequests[6] = new Friendshiprequest()
            {
                UserRecipientId = IdentityUsers[6].Id,
                UserTransmitterId = IdentityUsers[7].Id,
            };

            FriendshipRequests[7] = new Friendshiprequest()
            {
                UserRecipientId = IdentityUsers[7].Id,
                UserTransmitterId = IdentityUsers[8].Id,
            };

            FriendshipRequests[8] = new Friendshiprequest()
            {
                UserRecipientId = IdentityUsers[8].Id,
                UserTransmitterId = IdentityUsers[9].Id,
            };

            FriendshipRequests[9] = new Friendshiprequest()
            {
                UserRecipientId = IdentityUsers[9].Id,
                UserTransmitterId = IdentityUsers[0].Id,
            };

            // Save changes to the database
            await context.Friendshiprequests.AddRangeAsync(FriendshipRequests);
            await context.SaveChangesAsync();
        }

        public static async Task SeedFriendships(VibeNetDbContext context)
        {
            if (!context.Friendships.Any())
            {
                var friendshipRequests = context.Friendshiprequests.ToList();

                foreach (var request in friendshipRequests)
                {
                    Random random = new Random();
                    int months = random.Next(-40, 40);

                    var existingFriendship = context.Friendships.Any(f =>
                        (f.FirstUserId == request.UserTransmitterId && f.SecondUserId == request.UserRecipientId) ||
                        (f.FirstUserId == request.UserRecipientId && f.SecondUserId == request.UserTransmitterId));

                    if (!existingFriendship)
                    {
                        var friendship = new Friendship
                        {
                            FirstUserId = request.UserTransmitterId,
                            SecondUserId = request.UserRecipientId,
                            FriendsSince = DateTime.Parse("2011.11.04").AddMonths(months),
                        };

                        Friendships.Add(friendship);
                    }
                }

                await context.AddRangeAsync(Friendships);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedManagerRole(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, VibeNetDbContext context)
        {
            const string managerRole = AminRole;
            if (!await roleManager.RoleExistsAsync(managerRole))
                await roleManager.CreateAsync(new IdentityRole(managerRole));

            const string adminEmail = "admin@abv.com";
            const string adminPassword = "Admin123!";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, managerRole);
                }

                var profilePicture = new ProfilePicture
                {
                    Name = "admin",
                    ContentType = "jpg",
                    Data = await PictureHelper.ConvertToBytesAsync("admin.jpg")
                };

                await context.ProfilePictures.AddAsync(profilePicture);
                await context.SaveChangesAsync();

                var vibenetUser = new VibeNetUser()
                {
                    User = adminUser,
                    FirstName = "Admin",
                    LastName = "Adminov",
                    Birthday = DateTime.Parse("1996-04-15"),
                    CreatedOn = DateTime.Parse("2000-01-01"),
                    HomeTown = "Montana",
                    Gender = Gender.Male,
                    ProfilePictureId = profilePicture.Id,
                    IsDeleted = false,
                };

                await context.VibeNetUsers.AddAsync(vibenetUser);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedUserClaims(UserManager<IdentityUser> userManager)
        {
            UserClaims[0] = new IdentityUserClaim<string>()
            {
                ClaimType = UserFullNameClaim,
                ClaimValue = "Jane Smith",
                UserId = IdentityUsers[0].Id
            };

            UserClaims[1] = new IdentityUserClaim<string>()
            {
                ClaimType = UserFullNameClaim,
                ClaimValue = "Tom Garcia",
                UserId = IdentityUsers[1].Id
            };

            UserClaims[2] = new IdentityUserClaim<string>()
            {
                ClaimType = UserFullNameClaim,
                ClaimValue = "Nikolai Werner",
                UserId = IdentityUsers[2].Id
            };

            UserClaims[3] = new IdentityUserClaim<string>()
            {
                ClaimType = UserFullNameClaim,
                ClaimValue = "Alexandra Schmidt",
                UserId = IdentityUsers[3].Id
            };

            UserClaims[4] = new IdentityUserClaim<string>()
            {
                ClaimType = UserFullNameClaim,
                ClaimValue = "Daniel Murphy",
                UserId = IdentityUsers[4].Id
            };

            UserClaims[5] = new IdentityUserClaim<string>()
            {
                ClaimType = UserFullNameClaim,
                ClaimValue = "Emily Nguyen",
                UserId = IdentityUsers[5].Id
            };

            UserClaims[6] = new IdentityUserClaim<string>()
            {
                ClaimType = UserFullNameClaim,
                ClaimValue = "John Ivanov",
                UserId = IdentityUsers[6].Id
            };

            UserClaims[7] = new IdentityUserClaim<string>()
            {
                ClaimType = UserFullNameClaim,
                ClaimValue = "Michael Hernández",
                UserId = IdentityUsers[7].Id
            };

            UserClaims[8] = new IdentityUserClaim<string>()
            {
                ClaimType = UserFullNameClaim,
                ClaimValue = "Jane Smith",
                UserId = IdentityUsers[8].Id
            };

            UserClaims[9] = new IdentityUserClaim<string>()
            {
                ClaimType = UserFullNameClaim,
                ClaimValue = "Jane Smith",
                UserId = IdentityUsers[9].Id
            };

            foreach (var userClaim in UserClaims)
            {
                var user = await userManager.FindByIdAsync(userClaim.UserId);
                if (user != null)
                {
                    await userManager.AddClaimAsync(user, new System.Security.Claims.Claim(userClaim.ClaimType, userClaim.ClaimValue));
                }
            }
        }
    }
}

