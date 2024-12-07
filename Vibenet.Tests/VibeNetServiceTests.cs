using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Immutable;
using System.Linq.Expressions;
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

            vibeNetUser = AddUserAsync().Result;
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
        public async Task CreateVibeNetUserProfileViewModel_Should_Returs_Null()
        {
            userRepositoryMock
                .Setup(ur => ur.GetAllAttached())
                .Returns(vibeNetDbContext.VibeNetUsers);

            VibeNetUserProfileViewModel? actual = await vibeNetService.CreateVibeNetUserProfileViewModel("notexisting");
            Assert.That(actual, Is.Null);
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

        private async Task<VibeNetUser?> AddUserAsync()
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

        [Test]
        public async Task FindUsers_Should_Return_Filtered_Users_By_Category_FirstName()
        {
            userManagerMock
             .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
             .ReturnsAsync(IdentityResult.Success);

            var identityUser = new IdentityUser
            {
                UserName = "tina@example.com",
                Email = "tina@example.com",
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
                FirstName = "Tina",
                LastName = "Petrova",
                Birthday = DateTime.Parse("1998-09-19"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "Plovdiv",
                Gender = Gender.Female,
                ProfilePicture = profilePicture
            };

            vibeNetDbContext.VibeNetUsers.Add(user);
            vibeNetDbContext.SaveChanges();

            var searchTerm = "Tina";
            var category = "firstname";

            userRepositoryMock
                .Setup(ur => ur.GetAllAttached())
                .Returns(vibeNetDbContext.VibeNetUsers.Where(u => u.FirstName == searchTerm));

            var (resultUser, count) = await vibeNetService.FindUsers(searchTerm, category, null, 1, 10);

            Assert.IsNotNull(resultUser);
            Assert.AreEqual(1, count);
        }


        [Test]
        public async Task FindUsers_Should_Return_Filtered_Users_By_Category_Lastname()
        {
            userManagerMock
             .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
             .ReturnsAsync(IdentityResult.Success);

            var identityUser = new IdentityUser
            {
                UserName = "simo@example.com",
                Email = "simo@example.com",
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
                FirstName = "Simo",
                LastName = "Kirilov",
                Birthday = DateTime.Parse("1998-09-19"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "Plovdiv",
                Gender = Gender.Female,
                ProfilePicture = profilePicture
            };

            vibeNetDbContext.VibeNetUsers.Add(user);
            vibeNetDbContext.SaveChanges();

            var searchTerm = "Kirilov";
            var category = "lastname";

            userRepositoryMock
                .Setup(ur => ur.GetAllAttached())
                .Returns(vibeNetDbContext.VibeNetUsers.Where(u => u.LastName == searchTerm));

            var (resultUser, count) = await vibeNetService.FindUsers(searchTerm, category, null, 1, 10);

            Assert.IsNotNull(resultUser);
            Assert.AreEqual(1, count);
        }

        [Test]
        public async Task FindUsers_Should_Return_Filtered_Users_By_Category_Hometown()
        {
            userManagerMock
             .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
             .ReturnsAsync(IdentityResult.Success);

            var identityUser = new IdentityUser
            {
                UserName = "christina@example.com",
                Email = "christina@example.com",
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
                FirstName = "Christina",
                LastName = "Smith",
                Birthday = DateTime.Parse("1998-09-19"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "Montana",
                Gender = Gender.Female,
                ProfilePicture = profilePicture
            };

            vibeNetDbContext.VibeNetUsers.Add(user);
            vibeNetDbContext.SaveChanges();

            var searchTerm = "Montana";
            var category = "hometown";

            userRepositoryMock
                .Setup(ur => ur.GetAllAttached())
                .Returns(vibeNetDbContext.VibeNetUsers.Where(u => u.HomeTown == searchTerm));

            var (resultUser, count) = await vibeNetService.FindUsers(searchTerm, category, null, 1, 10);

            Assert.IsNotNull(resultUser);
            Assert.AreEqual(1, count);
        }


        [Test]
        public async Task FindUsers_Should_Return_Filtered_Users_By_Category_All()
        {
            userManagerMock
             .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
             .ReturnsAsync(IdentityResult.Success);

            var searchTerm = "gfdg";
            string category = null;

            userRepositoryMock
                .Setup(ur => ur.GetAllAttached())
                .Returns(vibeNetDbContext.VibeNetUsers.Where(u => u.HomeTown.ToLower() == searchTerm.ToLower()
                || u.FirstName.ToLower().Contains(searchTerm.ToLower())
                || u.LastName.ToLower().Contains(searchTerm.ToLower())));

            var (resultUser, count) = await vibeNetService.FindUsers(searchTerm, category, null, 1, 10);

            Assert.AreEqual(0, count);
        }

        [Test]
        public async Task FindUsers_Should_Return_Filtered_Users_By_Category_None()
        {
            userManagerMock
             .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
             .ReturnsAsync(IdentityResult.Success);

            var searchTerm = "notexisting";
            var category = "hometown";

            userRepositoryMock
                .Setup(ur => ur.GetAllAttached())
                .Returns(vibeNetDbContext.VibeNetUsers.Where(u => u.HomeTown == searchTerm));

            var (resultUser, count) = await vibeNetService.FindUsers(searchTerm, category, null, 1, 10);

            Assert.AreEqual(0, count);
        }

        [Test]
        public async Task DeleteAsync_Should_Delete_User_And_Profile_Picture()
        {
            userManagerMock
                .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var identityUser = new IdentityUser
            {
                UserName = "milo@example.com",
                Email = "milo@example.com",
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
            vibeNetDbContext.SaveChanges();  

            var user = new VibeNetUser()
            {
                User = identityUser,
                FirstName = "Milo",
                LastName = "Milov",
                Birthday = DateTime.Parse("1998-09-19"),
                CreatedOn = DateTime.Parse("2006-01-01"),
                HomeTown = "Bodenwöhr",
                Gender = Gender.Female,
                ProfilePicture = profilePicture,
                ProfilePictureId = profilePicture.Id 
            };

            vibeNetDbContext.VibeNetUsers.Add(user);
            vibeNetDbContext.SaveChanges();  

            userRepositoryMock
                .Setup(x => x.GetAllAttached())
                .Returns(vibeNetDbContext.VibeNetUsers);

            profilePictureServiceMock
                .Setup(x => x.Delete(It.IsAny<int>()))
                .Verifiable();

            userRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()))
                .Verifiable();


            await vibeNetService.DeleteAsync(user.IdentityUserId);  


            profilePictureServiceMock.Verify(x => x.Delete(profilePicture.Id), Times.Once);  
            userRepositoryMock.Verify(x => x.Delete(user.Id), Times.Once);  
        }

    }
}