using Microsoft.EntityFrameworkCore;
using Moq;
using VibeNet.Core.Services;
using VibeNet.Infrastucture.Data;
using VibeNet.Infrastucture.Data.Models;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNet.Infrastucture.Utilities;

namespace Vibenet.Tests
{
    public class ProfilePictureServiceTest
    {
        private Mock<IRepository<ProfilePicture, int>> profilePictureRepositoryMock;
        private VibeNetDbContext vibeNetDbContext;
        private ProfilePictureService profilePictureService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<VibeNetDbContext>()
                .UseInMemoryDatabase("VibeNetInMemoryDb")
                .Options;

            this.vibeNetDbContext = new VibeNetDbContext(options);

            profilePictureRepositoryMock = new Mock<IRepository<ProfilePicture, int>>();
            profilePictureService = new ProfilePictureService(profilePictureRepositoryMock.Object);
        }

        [Test]
        public async Task GetProfilePictureAsync_Should_Return_ViewModel_When_Picture_Found()
        {

            var profilePicture = new ProfilePicture
            {
                Name = "jane",
                ContentType = "jpeg",
                Data = await PictureHelper.ConvertToBytesAsync("jane.jpeg")
            };

            vibeNetDbContext.ProfilePictures.Add(profilePicture);
            vibeNetDbContext.SaveChanges();

            profilePictureRepositoryMock
                .Setup(repo => repo.GetAllAttached())
                .Returns(vibeNetDbContext.ProfilePictures);

            var result = await profilePictureService.GetProfilePictureAsync(profilePicture.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(profilePicture.Id, result.Id);
            Assert.AreEqual(profilePicture.ContentType, result.ContentType);
            Assert.AreEqual(profilePicture.Name, result.Name);
            Assert.AreEqual(profilePicture.Data, result.Data);
        }

        [TearDown]
        public void Teardown()
        {
            vibeNetDbContext.Dispose();
        }

    }
}
