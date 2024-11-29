using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using VibeNet.Core.Interfaces;
using VibeNet.Core.ViewModels;
using VibeNet.Infrastucture.Data;
using VibeNet.Infrastucture.Data.Models;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNet.Infrastucture.Utilities;
using VibeNetInfrastucture.Constants;
using VibeNetInfrastucture.Data.Models;
using VibeNetInfrastucture.Data.Models.Enums;

namespace Vibenet.Tests
{
    public class FriendshipServiceTests
    {
        private Mock<IRepository<Friendship, object>> friendshipRepositoryMock;
        private Mock<IVibeNetService> vibeNetServiceMock;
        private Mock<UserManager<IdentityUser>> userManagerMock;
        private VibeNetDbContext vibeNetDbContext;
        VibeNetUser? firstVibeNetUser;
        VibeNetUser? secondVibeNetUser;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<VibeNetDbContext>()
                .UseInMemoryDatabase("VibeNetInMemoryDb")
                .Options;

            var store = new Mock<IUserStore<IdentityUser>>();
            var userValidators = new List<IUserValidator<IdentityUser>>();
            var passwordValidators = new List<IPasswordValidator<IdentityUser>>();
            var passwordHasher = new Mock<IPasswordHasher<IdentityUser>>();
            var normalizer = new Mock<ILookupNormalizer>();
            var describer = new Mock<IdentityErrorDescriber>();

            userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object,
                null,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                normalizer.Object,
                describer.Object,
                null,
                null
            );

            this.vibeNetDbContext = new VibeNetDbContext(options);
            firstVibeNetUser = AddUser("Maria", "Ivanova").Result;
            secondVibeNetUser = AddUser("Sonia", "Petrova").Result;
        }

        [Test]
        public async Task GetFriendsAsync_ShouldReturnFriendsList()
        {
            if (secondVibeNetUser == null) return;

            var friend = new VibeNetUserProfileViewModel()
            {
                Id = secondVibeNetUser.Id,
                IdentityId = secondVibeNetUser.IdentityUserId,
                FirstName = secondVibeNetUser.FirstName,
                LastName = secondVibeNetUser.LastName,
                Gender = secondVibeNetUser.Gender,
                HomeTown = secondVibeNetUser.HomeTown,
                Birthday = secondVibeNetUser.Birthday.ToString(Validations.DateTimeFormat.Format),
                ProfilePicture = null
            };

            var friendship = new Friendship()
            {
                FirstUserId = firstVibeNetUser.IdentityUserId,
                SecondUserId = secondVibeNetUser.IdentityUserId,
                FriendsSince = DateTime.Now
            };

            friendshipRepositoryMock
                .Setup(fr => fr.GetAllAttached())
                .Returns(new List<Friendship>() { friendship }.AsQueryable);
        }

        [TearDown]
        public void Teardown()
        {
            vibeNetDbContext.Dispose();
        }

        private async Task<VibeNetUser?> AddUser(string firstname, string lastname)
        {
            userManagerMock
             .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
             .ReturnsAsync(IdentityResult.Success);

            var identityUser = new IdentityUser
            {
                UserName = $"{firstname}@example.com",
                Email = $"{firstname}@example.com",
                EmailConfirmed = true
            };

            var result = await userManagerMock.Object.CreateAsync(identityUser, "Password123!");

            var profilePicture = new ProfilePicture
            {
                Name = "jane",
                ContentType = "jpeg",
                Data = await PictureHelper.ConvertToBytesAsync("jane.jpeg")
            };

            vibeNetDbContext.ProfilePictures.Add(profilePicture);

            var user = new VibeNetUser()
            {
                User = identityUser,
                FirstName = firstname,
                LastName = lastname,
                Birthday = DateTime.Parse("1998-09-19"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "Sofia",
                Gender = Gender.Female,
                ProfilePicture = profilePicture
            };

            vibeNetDbContext.VibeNetUsers.Add(user);
            vibeNetDbContext.SaveChanges();

            return user;
        }
    }
}
