using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using VibeNet.Core.Contracts;
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
    public class LikeServiceTests
    {
        private Mock<IRepository<Like, int>> likeRepositoryMock;
        private Mock<IPostService> postServiceMock;
        private Mock<UserManager<IdentityUser>> userManagerMock;
        private LikeService likeService;
        private VibeNetDbContext vibeNetDbContext;
        VibeNetUser? vibeNetUser;

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
            postServiceMock = new Mock<IPostService>();
            likeRepositoryMock = new Mock<IRepository<Like, int>>();
            likeService = new LikeService(likeRepositoryMock.Object, postServiceMock.Object);

            vibeNetUser = AddUser().Result;

        }

        [Test]
        public async Task AddLikeAsync_ShouldReturn_False()
        {
            Post post = new Post()
            {
                OwnerId = vibeNetUser.IdentityUserId,
                Content = "How are you?",
                PostedOn = DateTime.Now,
            };

            vibeNetDbContext.Posts.Add(post);
            vibeNetDbContext.SaveChanges();

            var vibeNetUserProfileViewModel = new VibeNetUserProfileViewModel()
            {
                Id = vibeNetUser.Id,
                IdentityId = vibeNetUser.IdentityUserId,
                FirstName = vibeNetUser.FirstName,
                LastName = vibeNetUser.LastName,
                Gender = vibeNetUser.Gender,
                HomeTown = vibeNetUser.HomeTown,
                Birthday = vibeNetUser.Birthday.ToString(Validations.DateTimeFormat.Format),
                ProfilePicture = null
            };

            var likeViewModel = new LikeViewModel()
            {
                Owner = vibeNetUserProfileViewModel,
                Id = 1
            };

            var model = new PostViewModel()
            {
                Id = post.Id,
                OwnerId = post.OwnerId,
                Content = post.Content,
                PostedOn = post.PostedOn,
                UserLiked = new List<LikeViewModel>(),
                Comments = new List<CommentViewModel>()
            };

            var like = new Like()
            {
                PostId = post.Id,
                OwnerId = model.OwnerId,
            };

            vibeNetDbContext.Likes.Add(like);
            vibeNetDbContext.SaveChanges();

            model.UserLiked.Add(likeViewModel);

            postServiceMock
                .Setup(p => p.GetByIdAsync(post.Id).Result)
                .Returns(model);

            likeRepositoryMock
                .Setup(l => l.GetAllAsync().Result)
                .Returns(vibeNetDbContext.Likes.ToList());

            var result = await likeService.AddLikeAsync(post.Id, likeViewModel, vibeNetUser.IdentityUserId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddLikeAsync_ShouldReturn_True()
        {
            Post post = new Post()
            {
                OwnerId = vibeNetUser.IdentityUserId,
                Content = "How are you?",
                PostedOn = DateTime.Now,
            };

            vibeNetDbContext.Posts.Add(post);
            vibeNetDbContext.SaveChanges();

            var vibeNetUserProfileViewModel = new VibeNetUserProfileViewModel()
            {
                Id = vibeNetUser.Id,
                IdentityId = vibeNetUser.IdentityUserId,
                FirstName = vibeNetUser.FirstName,
                LastName = vibeNetUser.LastName,
                Gender = vibeNetUser.Gender,
                HomeTown = vibeNetUser.HomeTown,
                Birthday = vibeNetUser.Birthday.ToString(Validations.DateTimeFormat.Format),
                ProfilePicture = null
            };


            var likeViewModel = new LikeViewModel()
            {
                Owner = vibeNetUserProfileViewModel,
                Id = 1
            };

            var model = new PostViewModel()
            {
                Id = post.Id,
                OwnerId = post.OwnerId,
                Content = post.Content,
                PostedOn = post.PostedOn,
                UserLiked = new List<LikeViewModel>(),
                Comments = new List<CommentViewModel>()
            };

            postServiceMock
                .Setup(p => p.GetByIdAsync(post.Id).Result)
                .Returns(model);

            likeRepositoryMock
                .Setup(l => l.GetAllAsync().Result)
                .Returns(vibeNetDbContext.Likes.ToList());

            var result = await likeService.AddLikeAsync(post.Id, likeViewModel, vibeNetUser.IdentityUserId);

            Assert.IsTrue(result);
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
                Data = await PictureFileHelper.ConvertToBytesAsync("jane.jpeg")
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

