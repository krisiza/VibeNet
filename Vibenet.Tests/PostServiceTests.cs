using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using VibeNet.Core.Contracts;
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
    public class PostServiceTests
    {
        private Mock<IRepository<Post, int>> postRepositoryMock;
        private Mock<IRepository<Comment, int>> commentRepositoryMock;
        private Mock<IRepository<Like, int>> likeRepositoryMock;
        private Mock<IRepository<Friendship, object>> friendshipRepositoryMock;
        private Mock<IRepository<VibeNetUser, int>> userRepositoryMock;
        private Mock<IVibeNetService> vibeNetService;
        private Mock<IFriendshipService> friendshipService;
        private PostService postService;
        private Mock<UserManager<IdentityUser>> userManagerMock;
        private VibeNetDbContext vibeNetDbContext;
        VibeNetUser? vibeNetUser;
        VibeNetUserProfileViewModel? vibeNetUserProfileViewModel;
        ProfilePictureViewModel? profilePictureViewModel;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<VibeNetDbContext>()
                .UseInMemoryDatabase("VibeNetInMemoryDb")
                .Options;

            this.vibeNetDbContext = new VibeNetDbContext(options);

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

            userRepositoryMock = new Mock<IRepository<VibeNetUser, int>>();
            postRepositoryMock = new Mock<IRepository<Post, int>>();
            commentRepositoryMock = new Mock<IRepository<Comment, int>>();
            friendshipRepositoryMock = new Mock<IRepository<Friendship, object>>();
            likeRepositoryMock = new Mock<IRepository<Like, int>>();

            vibeNetService = new Mock<IVibeNetService>();
            friendshipService = new Mock<IFriendshipService>();
            postService = new PostService(postRepositoryMock.Object, commentRepositoryMock.Object,
                vibeNetService.Object, userManagerMock.Object, friendshipService.Object, likeRepositoryMock.Object);

            vibeNetUser = AddUser().Result;
        }

        [Test]
        public async Task GetFriendsPostsAsync_Should_Returns_PostViewModel_List()
        {
            // Arrange
            var post = new Post()
            {
                OwnerId = vibeNetUser.IdentityUserId,
                Content = "How are you?",
                PostedOn = DateTime.Now,
            };

            var model = await CreatePostViewModel(post);
            vibeNetDbContext.Posts.Add(post);
            await vibeNetDbContext.SaveChangesAsync();

            List<VibeNetUserProfileViewModel> friends = new List<VibeNetUserProfileViewModel> { vibeNetUserProfileViewModel };

            friendshipService
                .Setup(fr => fr.GetFriendsAsync(vibeNetUser.IdentityUserId))
                .ReturnsAsync(friends);

            postRepositoryMock
                .Setup(p => p.GetAllAttached())
                .Returns(vibeNetDbContext.Posts.AsQueryable()); // Mock posts retrieval

            vibeNetService
                .Setup(vs => vs.CreateVibeNetUserProfileViewModel(vibeNetUser.IdentityUserId))
                .ReturnsAsync(vibeNetUserProfileViewModel);

            // Act
            var models = await postService.GetFriendsPostsAsync(vibeNetUser.IdentityUserId);

            // Assert
            Assert.That(models, Has.Some.Matches<PostViewModel>(m =>
                     m.OwnerId == model.OwnerId &&
                     m.Content == model.Content &&
                     m.PostedOn == model.PostedOn
                 ));
        }

        [Test]
        public async Task GetAllAsync_Should_Return_Entities()
        {
            var post = new Post()
            {
                OwnerId = vibeNetUser.IdentityUserId,
                Content = "How are you?",
                PostedOn = DateTime.Now,
            };

            var model = await CreatePostViewModel(post);

            vibeNetDbContext.Posts.Add(post);
            vibeNetDbContext.SaveChanges();

            postRepositoryMock
                .Setup(p => p.GetAllAttached())
                .Returns(vibeNetDbContext.Posts);

            vibeNetService
                .Setup(vs => vs.CreateVibeNetUserProfileViewModel(vibeNetUser.IdentityUserId))
                .ReturnsAsync(vibeNetUserProfileViewModel);

            var models = await postService.GetAllAsync(vibeNetUser.IdentityUserId);

            Assert.That(models, Has.Some.Matches<PostViewModel>(m =>
                     m.OwnerId == model.OwnerId &&
                     m.Content == model.Content &&
                     m.PostedOn == model.PostedOn
                 ));
        }


        [Test]
        public async Task GetByIdAsync_Should_Return_PostViewModel()
        {
            var post = new Post()
            {
                OwnerId = vibeNetUser.IdentityUserId,
                Content = "How are you?",
                PostedOn = DateTime.Now,
            };

            var model = await CreatePostViewModel(post);
            vibeNetDbContext.Posts.Add(post);
            vibeNetDbContext.SaveChanges();

            postRepositoryMock
                .Setup(p => p.GetByIdAsync(post.Id))
                .ReturnsAsync(post);

            vibeNetService
               .Setup(vs => vs.CreateVibeNetUserProfileViewModel(vibeNetUser.IdentityUserId))
               .ReturnsAsync(vibeNetUserProfileViewModel);

            var actual = await postService.GetByIdAsync(post.Id);

            Assert.That(actual, Is.Not.Null);
            Assert.IsTrue(actual.Owner == model.Owner && actual.Content == model.Content);
        }

        [Test]
        public async Task AddPostAsync_Should_Create_Post()
        {
            // Arrange
            var postContent = "New Post Content";
            var userId = vibeNetUser.IdentityUserId;

            postRepositoryMock
                .Setup(p => p.AddAsync(It.IsAny<Post>()))
                .ReturnsAsync(1);  // Simulating async add operation

            // Act
            await postService.AddPostAsync(postContent, userId);

            // Assert
            postRepositoryMock.Verify(p => p.AddAsync(It.Is<Post>(p =>
                p.Content == postContent &&
                p.OwnerId == userId
            )), Times.Once);
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

        private async Task<PostViewModel> CreatePostViewModel(Post post)
        {
            var profilePicture = new ProfilePicture
            {
                Name = "jane",
                ContentType = "jpeg",
                Data = await PictureFileHelper.ConvertToBytesAsync("jane.jpeg")
            };

            ProfilePictureViewModel profilePictureViewModel = CreateProfilePictureViewModel(profilePicture);

            var userViewModel = CreateVebeNetUserViewModel(profilePictureViewModel);

            var model = new PostViewModel()
            {
                Id = post.Id,
                OwnerId = post.OwnerId,
                Owner = userViewModel,
                Content = post.Content,
                PostedOn = post.PostedOn,
                UserLiked = new List<LikeViewModel>(),
                Comments = new List<CommentViewModel>()
            };

            return model;
        }

        private VibeNetUserProfileViewModel CreateVebeNetUserViewModel(ProfilePictureViewModel profilePictureViewModel)
        {
            vibeNetUserProfileViewModel = new VibeNetUserProfileViewModel()
            {
                Id = vibeNetUser.Id,
                IdentityId = vibeNetUser.IdentityUserId,
                FirstName = vibeNetUser.FirstName,
                LastName = vibeNetUser.LastName,
                Gender = vibeNetUser.Gender,
                HomeTown = vibeNetUser.HomeTown,
                Birthday = vibeNetUser.Birthday.ToString(Validations.DateTimeFormat.Format),
                ProfilePicture = profilePictureViewModel
            };

            return vibeNetUserProfileViewModel;
        }

        private ProfilePictureViewModel CreateProfilePictureViewModel(ProfilePicture profilePicture)
        {
            profilePictureViewModel = new ProfilePictureViewModel()
            {
                Id = profilePicture.Id,
                ContentType = profilePicture.ContentType,
                Data = profilePicture.Data,
                Name = profilePicture.Name,
            };

            return profilePictureViewModel;
        }
    }
}
