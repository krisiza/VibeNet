using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Vibenet.Tests.CustomComparers;
using VibeNet.Core.Contracts;
using VibeNet.Core.Services;
using VibeNet.Core.Utilities;
using VibeNet.Core.ViewModels;
using VibeNet.Infrastucture.Data;
using VibeNet.Infrastucture.Data.Models;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNet.Infrastucture.Utilities;
using VibeNetInfrastucture.Constants;
using VibeNetInfrastucture.Data.Models;
using VibeNetInfrastucture.Data.Models.Enums;
using PictureHelper = VibeNet.Core.Utilities.PictureHelper;

namespace Vibenet.Tests
{
    [TestFixture]
    public class Tests
    {
        private Mock<IRepository<VibeNetUser, int>> userRepositoryMock;
        private Mock<IProfilePictureService> profilePictureServiceMock;
        private Mock<UserManager<IdentityUser>> userManagerMock;
        private Mock<IPictureHelper> pictureHelper;
        private VibeNetService vibeNetService;
        private VibeNetDbContext vibeNetDbContext;
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

            pictureHelper = new Mock<IPictureHelper>();

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

            vibeNetService = new VibeNetService(userRepositoryMock.Object, profilePictureServiceMock.Object, 
                pictureHelper.Object);

            vibeNetUser = AddUser().Result;
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

        [Test]
        public async Task GetByIdAsync_Should_Return_User()
        {
            userRepositoryMock
                .Setup(ur => ur.GetAllAttached())
                .Returns(vibeNetDbContext.VibeNetUsers);

            var actualUser = await vibeNetService.GetByIdAsync(vibeNetUser.Id);
            Assert.That(actualUser, Is.SameAs(vibeNetUser));
        }

        [Test]
        public async Task CreateVibeNetUserProfileViewModel_Should_Create_Model()
        {
            userRepositoryMock
                .Setup(ur => ur.GetAllAttached())
                .Returns(vibeNetDbContext.VibeNetUsers);

            var user = await vibeNetService.GetByIdentityIdAsync(vibeNetUser.IdentityUserId);

            var profilePicture = new ProfilePicture
            {
                Name = "jane",
                ContentType = "jpeg",
                Data = await VibeNet.Infrastucture.Utilities.PictureFileHelper.ConvertToBytesAsync("jane.jpeg")
            };

            ProfilePictureViewModel profilePictureViewModel = new ProfilePictureViewModel()
            {
                Id = profilePicture.Id,
                ContentType = profilePicture.ContentType,
                Data = profilePicture.Data,
                Name = profilePicture.Name,
            };

            profilePictureServiceMock
                .Setup(pp => pp.GetProfilePictureAsync(user.ProfilePictureId).Result)
                .Returns(profilePictureViewModel);

            VibeNetUserProfileViewModel? model = new VibeNetUserProfileViewModel
            {
                Id = user.Id,
                IdentityId = user.IdentityUserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                HomeTown = user.HomeTown,
                Birthday = user.Birthday.ToString(Validations.DateTimeFormat.Format),
                ProfilePicture = profilePictureViewModel
            };

            VibeNetUserProfileViewModel? actual = await vibeNetService.CreateVibeNetUserProfileViewModel(user.IdentityUserId);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.EqualTo(model).Using(new VibeNetUserProfileViewModelComparer()));
        }

        [Test]
        public async Task CreateFormUserViewModel_Should_Create_Model()
        {
            userRepositoryMock
                .Setup(ur => ur.GetAllAttached())
                .Returns(vibeNetDbContext.VibeNetUsers);

            var user = await vibeNetService.GetByIdentityIdAsync(vibeNetUser.IdentityUserId);

            VibeNetUserFormViewModel? model = new VibeNetUserFormViewModel
            {

                Id = Guid.Parse(user.IdentityUserId),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                HomeTown = user.HomeTown,
                Birthday = user.Birthday.ToString(Validations.DateTimeFormat.Format)
            };

            VibeNetUserFormViewModel? actual = await vibeNetService.CreateFormUserViewModel(user.IdentityUserId);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.EqualTo(model).Using(new VibeNetUserFormViewModelComparer()));
        }

        [Test]
        public async Task UpdateAsync_Should_Update_User()
        {
            vibeNetUser.FirstName = "Kristiyana";
            await vibeNetService.UpdateAsync(vibeNetUser);

            var foundUser = vibeNetDbContext.VibeNetUsers.FirstOrDefault(u => u.Id == vibeNetUser.Id);
            Assert.IsTrue("Kristiyana" == foundUser.FirstName);
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
                Data = await VibeNet.Infrastucture.Utilities.PictureFileHelper.ConvertToBytesAsync("jane.jpeg")
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