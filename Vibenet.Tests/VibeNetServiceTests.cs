using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using VibeNet.Core.Contracts;
using VibeNet.Core.Services;
using VibeNet.Infrastucture.Data;
using VibeNet.Infrastucture.Data.Models;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNet.Infrastucture.Utilities;
using VibeNetInfrastucture.Data.Models;
using VibeNetInfrastucture.Data.Models.Enums;

namespace Vibenet.Tests
{
    [TestFixture]
    public class Tests
    {
        private Mock<IRepository<VibeNetUser, int>> userRepositoryMock;
        private Mock<IProfilePictureService> profilePictureServiceMock;
        private Mock<UserManager<IdentityUser>> userManagerMock;
        private VibeNetService vibeNetService;
        private VibeNetDbContext vibeNetDbContext;
        private IEnumerable<VibeNetUser> users;
        VibeNetUser? vibeNetUser;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<VibeNetDbContext>()
                .UseInMemoryDatabase("VibeNetInMemoryDb")
                .Options;

            this.vibeNetDbContext = new VibeNetDbContext(options);

            userRepositoryMock = new Mock<IRepository<VibeNetUser, int>>();
            profilePictureServiceMock = new Mock<IProfilePictureService>();

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

            vibeNetService = new VibeNetService(userRepositoryMock.Object, profilePictureServiceMock.Object);

            vibeNetUser = AddUser().Result;
        }

        [Test]
        public void AddUserAsync_Should_Add_User()
        {
            Assert.IsNotNull(vibeNetUser);
            Assert.IsTrue(vibeNetDbContext.VibeNetUsers.Contains(vibeNetUser));
        }

        [Test]
        public async Task GetByIdentityId_Should_Return_User()
        {
            userRepositoryMock
                .Setup(ur => ur.GetAllAttached())
                .Returns(vibeNetDbContext.VibeNetUsers);

            var actualUser = await vibeNetService.GetByIdentityIdAsync(vibeNetUser.IdentityUserId);
            Assert.That(actualUser, Is.SameAs(vibeNetUser));
        }

        [TearDown]
        public void Teardown()
        {
            vibeNetDbContext.Dispose();
        }

        private async Task<VibeNetUser?> AddUser()
        {
            userManagerMock
             .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
             .ReturnsAsync(IdentityResult.Success);

            var identityUser = new IdentityUser
            {
                UserName = "maria@example.com",
                Email = "maria@example.com",
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
                FirstName = "Maria",
                LastName = "Ivanova",
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