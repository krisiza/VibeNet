using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.Services;
using VibeNet.Core.ViewModels;
using VibeNet.Infrastucture.Data;
using VibeNet.Infrastucture.Data.Models;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNet.Infrastucture.Utilities;
using VibeNetInfrastucture.Data.Models;
using VibeNetInfrastucture.Data.Models.Enums;

namespace Vibenet.Tests
{
    public class FriendshiprequestServiceTests
    {
        private Mock<IRepository<Friendshiprequest, object>> friendshipRequestRepositoryMock;
        private Mock<IVibeNetService> vibeNetServiceMock;
        private Mock<IFriendshipService> friendshipServiceMock;
        private Mock<UserManager<IdentityUser>> userManagerMock;
        private VibeNetDbContext vibeNetDbContext;
        private VibeNetUser? firstVibeNetUser;
        private IdentityUser? firstIdentityUser;
        private IdentityUser? secondIdentityUser;
        private VibeNetUser? secondVibeNetUser;
        private FriendshiprequestService friendshiprequestService;
        private Friendshiprequest friendshiprequest;


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
            friendshipRequestRepositoryMock = new Mock<IRepository<Friendshiprequest, object>>();
            vibeNetServiceMock = new Mock<IVibeNetService>();
            friendshipServiceMock = new Mock<IFriendshipService>();
            friendshiprequestService = new FriendshiprequestService(friendshipRequestRepositoryMock.Object, vibeNetServiceMock.Object
                , friendshipServiceMock.Object);

            firstIdentityUser = CreateIdentityUser("Maria", "Ivanova").Result;
            secondIdentityUser = CreateIdentityUser("Sonia", "Petrova").Result;
            firstVibeNetUser = CreateVibeNetUser("Maria", "Ivanova", firstIdentityUser).Result;
            secondVibeNetUser = CreateVibeNetUser("Sonia", "Petrova", secondIdentityUser).Result;

            friendshiprequest = new Friendshiprequest()
            {
                UserRecipient = firstIdentityUser,
                UserTransmitter = secondIdentityUser,
            };

            vibeNetDbContext.Friendshiprequests.Add(friendshiprequest);
            vibeNetDbContext.SaveChanges();
        }

        [Test]
        public async Task FindByIdAsync_ShouldReturnTrue_WhenRequestExists()
        {
            var requestList = new List<Friendshiprequest> { friendshiprequest };
            friendshipRequestRepositoryMock
                .Setup(repo => repo.GetAllAttached())
                .Returns(vibeNetDbContext.Friendshiprequests);

            var result = await friendshiprequestService.FindByIdAsync(firstIdentityUser.Id, secondIdentityUser.Id);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task SendRequestAsync_ShouldSendRequest()
        {
            await friendshiprequestService.SendRequestAsync(firstIdentityUser.Id, secondIdentityUser.Id);
            friendshipRequestRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Friendshiprequest>()), Times.Once);
        }

        [Test]
        public async Task AcceptRequest_ShouldReturn_Null()
        {

            friendshipRequestRepositoryMock
                .Setup(repo => repo.GetAllAttached())
                .Returns(vibeNetDbContext.Friendshiprequests);

            friendshipServiceMock
                .Setup(service => service.AddFriendShipAsync(It.IsAny<Friendshiprequest>()))
                .Returns(Task.CompletedTask);

            await friendshiprequestService.AcceptRequest("notExistinfId2", "notExistinfId1");

            friendshipRequestRepositoryMock.Verify(repo => repo.DeleteEntityAsync(It.IsAny<Friendshiprequest>()), Times.Never);
            friendshipServiceMock.Verify(service => service.AddFriendShipAsync(It.IsAny<Friendshiprequest>()), Times.Never);
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
