using Microsoft.AspNetCore.Http;
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
                Data = await PictureFileHelper.ConvertToBytesAsync("jane.jpeg")
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

        [Test]
        public async Task SavePictureAsync_Should_Save_ProfilePicture_When_FormFile_Is_Null()
        {
            byte[] sampleData = new byte[] { 0x01, 0x02, 0x03 }; 
            IFormFile? formFile = null; 

            profilePictureRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<ProfilePicture>()))
                .ReturnsAsync(1); 

            var result = await profilePictureService.SavePictureAsync(formFile, sampleData);

            profilePictureRepositoryMock.Verify(repo => repo.AddAsync(It.Is<ProfilePicture>(p =>
                p.Name == "profile-avatar" &&
                p.ContentType == ".jpg" &&
                p.Data == sampleData
            )), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual("profile-avatar", result.Name);
            Assert.AreEqual(".jpg", result.ContentType);
            Assert.AreEqual(sampleData, result.Data);
        }


        [Test]
        public async Task GetProfilePictureAsync_Should_Return_ViewModel_When_Picture_Not_Found()
        {
            profilePictureRepositoryMock
                .Setup(repo => repo.GetAllAttached())
                .Returns(vibeNetDbContext.ProfilePictures);

            var result = await profilePictureService.GetProfilePictureAsync(342342);
            Assert.IsNull(result);

        }

        [TearDown]
        public void Teardown()
        {
            vibeNetDbContext.Dispose();
        }

    }
}
