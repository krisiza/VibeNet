using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using VibeNet.Core.Interfaces;
using VibeNet.Core.Services;
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
        private VibeNetUser? firstVibeNetUser;
        private IdentityUser? firstIdentityUser;
        private IdentityUser? secondIdentityUser;
        private VibeNetUser? secondVibeNetUser;
        private FriendshipService friendshipService;

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
            friendshipRepositoryMock = new Mock<IRepository<Friendship, object>>();
            vibeNetServiceMock = new Mock<IVibeNetService>();
            friendshipService = new FriendshipService(friendshipRepositoryMock.Object, vibeNetServiceMock.Object);

            firstIdentityUser = CreateIdentityUser("Maria", "Ivanova").Result;
            secondIdentityUser = CreateIdentityUser("Sonia", "Petrova").Result;
            firstVibeNetUser = CreateVibeNetUser("Maria", "Ivanova", firstIdentityUser).Result;
            secondVibeNetUser = CreateVibeNetUser("Sonia", "Petrova", secondIdentityUser).Result;
        }

        [Test]
        public async Task GetFriendsAsync_ShouldReturnFriendsList()
        {
            var friendship = new Friendship()
            {
                FirstUser = firstIdentityUser,
                SecondUser = secondIdentityUser,
                FriendsSince = DateTime.Now
            };

            vibeNetDbContext.Friendships.Add(friendship);
            vibeNetDbContext.SaveChanges();

            friendshipRepositoryMock
                .Setup(fr => fr.GetAllAttached())
                .Returns(vibeNetDbContext.Friendships );

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

            vibeNetServiceMock
                .Setup(v => v.CreateVibeNetUserProfileViewModel(friend.IdentityId))
                .ReturnsAsync(friend);

            var friendsList =await friendshipService.GetFriendsAsync(firstVibeNetUser.IdentityUserId);

            Assert.IsNotNull(friendsList);
            Assert.IsTrue(friendsList.Count() == 1);
        }

        [Test]
        public async Task FindByIdAsync_Should_Return_True()
        {
            var friendship = new Friendship()
            {
                FirstUser = firstIdentityUser,
                SecondUser = secondIdentityUser,
                FriendsSince = DateTime.Now
            };

            vibeNetDbContext.Friendships.Add(friendship);
            vibeNetDbContext.SaveChanges();

            friendshipRepositoryMock
                .Setup(fr => fr.GetAllAttached())
                .Returns(vibeNetDbContext.Friendships);

            var result = await friendshipService.FindByIdAsync(firstIdentityUser.Id, secondIdentityUser.Id);

            Assert.IsTrue(result);
        }


        [Test]
        public async Task FindByIdAsync_Should_Return_False()
        {
            friendshipRepositoryMock
                .Setup(fr => fr.GetAllAttached())
                .Returns(vibeNetDbContext.Friendships);

            var result = await friendshipService.FindByIdAsync(firstIdentityUser.Id, secondIdentityUser.Id);

            Assert.IsFalse(result);
        }

        [TearDown]
        public void Teardown()
        {
            vibeNetDbContext.Dispose();
        }

        private async Task<VibeNetUser?> CreateVibeNetUser(string firstname, string lastname, IdentityUser identityUser)
        {
            var profilePicture = new ProfilePicture
            {
                Name = "jane",
                ContentType = "jpeg",
                Data = await PictureFileHelper.ConvertToBytesAsync("jane.jpeg")
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

        private async Task<IdentityUser?> CreateIdentityUser(string firstname, string lastname)
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

            return identityUser;
        }
    }
}
