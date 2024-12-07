using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.Utilities;
using VibeNet.Core.ViewModels;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNetInfrastucture.Constants;
using VibeNetInfrastucture.Data.Models;
using static VibeNetInfrastucture.Constants.Validations;

namespace VibeNet.Core.Services
{
    public class VibeNetService : IVibeNetService
    {
        private readonly IRepository<VibeNetUser, int> userRepository;
        private readonly IProfilePictureService profilePictureService;
        private readonly IPictureHelper pictureHelper;
        public VibeNetService(IRepository<VibeNetUser, int> userRepository, IProfilePictureService profilePictureService,
             IPictureHelper pictureHelper)

        {
            this.userRepository = userRepository;
            this.profilePictureService = profilePictureService;
            this.pictureHelper = pictureHelper;
        }

        public async Task<int> AddUserAsync(VibeNetUserFormViewModel model)
        {
            byte[] data = await pictureHelper.ConvertToBytesAsync(model.ProfilePictureFile);
            model.Birthday = Convert.ToDateTime(model.Birthday).ToString(DateTimeFormat.Format);

            VibeNetUser user = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                HomeTown = model.HomeTown,
                Birthday = DateTime.ParseExact(model.Birthday, DateTimeFormat.Format, CultureInfo.InvariantCulture),
                CreatedOn = string.IsNullOrEmpty(model.CreatedOn)
                    ? DateTime.Now
                    : DateTime.ParseExact(model.CreatedOn, DateTimeFormat.Format, CultureInfo.InvariantCulture),
                Gender = model.Gender,
                IdentityUserId = model.Id.ToString(),
                ProfilePicture = await profilePictureService.SavePictureAsync(model.ProfilePictureFile, data)
            };

            return await userRepository.AddAsync(user);
        }

        public Task<VibeNetUser?> GetByIdentityIdAsync(string userIdentityId)
        {
            var user = userRepository.GetAllAttached()
                .Where(u => u.IdentityUserId == userIdentityId)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<VibeNetUser?> GetByIdAsync(int id)
        {
            var user = await userRepository.GetAllAttached()
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> UpdateAsync(VibeNetUser item)
            => await userRepository.UpdateAsync(item);

        public async Task<VibeNetUserProfileViewModel?> CreateVibeNetUserProfileViewModel(string userId)
        {
            var user = await GetByIdentityIdAsync(userId);

            if (user == null) return null;

            var model = new VibeNetUserProfileViewModel
            {
                Id = user.Id,
                IdentityId = user.IdentityUserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                HomeTown = user.HomeTown,
                Birthday = user.Birthday.ToString(Validations.DateTimeFormat.Format),
                ProfilePicture = await profilePictureService.GetProfilePictureAsync(user.ProfilePictureId)
            };

            return model;
        }

        public async Task<VibeNetUserFormViewModel?> CreateFormUserViewModel(string userId)
        {
            var user = await GetByIdentityIdAsync(userId);

            if (user == null) return null;

            var profilePicture = await profilePictureService.GetProfilePictureAsync(user.ProfilePictureId);

            var model = new VibeNetUserFormViewModel
            {
                Id = Guid.Parse(user.IdentityUserId),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                HomeTown = user.HomeTown,
                Birthday = user.Birthday.ToString(Validations.DateTimeFormat.Format)
            };

            return model;
        }

        public async Task DeleteAsync(string userId)
        {
            var user = await GetByIdentityIdAsync(userId);
            profilePictureService.Delete(user.ProfilePictureId);
            userRepository.Delete(user.Id);
        }

        public async Task<(IEnumerable<VibeNetUserProfileViewModel> Users, int TotalCount)> FindUsers(string searchedTerm, string? category, string userId, int pageNumber, int pageSize)
        {
            IQueryable<VibeNetUser> query;

            if (category == "firstname")
            {
                query = userRepository.GetAllAttached()
                    .Where(u => (u.FirstName.ToLower().Contains(searchedTerm.ToLower()))
                              && u.IdentityUserId != userId);
            }
            else if(category == "lastname")
            {
                query = userRepository.GetAllAttached()
                    .Where(u => (u.LastName.ToLower().Contains(searchedTerm.ToLower()))
                              && u.IdentityUserId != userId);
            }
            else if(category == "hometown")
            {
                query = userRepository.GetAllAttached()
                    .Where(u => (u.HomeTown.ToLower().Contains(searchedTerm.ToLower()))
                              && u.IdentityUserId != userId);
            }
            else
            {
                query = userRepository.GetAllAttached()
                    .Where(u => (u.FirstName.ToLower().Contains(searchedTerm.ToLower())
                              || u.LastName.ToLower().Contains(searchedTerm.ToLower())
                              || (u.HomeTown != null && u.HomeTown.ToLower().Contains(searchedTerm.ToLower())))
                              && u.IdentityUserId != userId);
            }

            int totalCount = await query.CountAsync();

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var models = new List<VibeNetUserProfileViewModel>();

            foreach (var user in users)
            {
                var model = new VibeNetUserProfileViewModel
                {
                    Id = user.Id,
                    IdentityId = user.IdentityUserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    HomeTown = user.HomeTown,
                    Birthday = user.Birthday.ToString(DateTimeFormat.Format),
                    ProfilePicture = await profilePictureService.GetProfilePictureAsync(user.ProfilePictureId)
                };

                models.Add(model);
            }

            return (models, totalCount);
        }
    }
}
