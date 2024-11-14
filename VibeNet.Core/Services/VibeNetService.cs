using VibeNet.Core.Interfaces;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNet.Core.ViewModels;
using VibeNetInfrastucture.Data.Models;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using VibeNet.Core.Contracts;
using VibeNet.Core.Utilities;

namespace VibeNet.Core.Services
{
    public class VibeNetService : IVibeNetService
    {
        private readonly IRepository<VibeNetUser, int> userRepository;
        private readonly IProfilePictureService profilePictureService;
        public VibeNetService(IRepository<VibeNetUser, int> userRepository, IProfilePictureService profilePictureService)

        {
            this.userRepository = userRepository;
            this.profilePictureService = profilePictureService;
        }

        public async Task AddUserAsync(VibeNetUserRegisterViewModel model)
        {

            byte[] data = await VibeNetHepler.ConvertToBytesAsync(model.ProfilePictureFile);

            VibeNetUser user = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                HomeTown = model.HomeTown,
                Birthday = Convert.ToDateTime(model.Birthday, CultureInfo.InvariantCulture),
                CreatedOn = Convert.ToDateTime(model.CreatedOn, CultureInfo.InvariantCulture),
                Gender = model.Gender,
                IsDeleted = model.IsDeleted,
                VibeNetUserId = model.Id.ToString(),
                ProfilePicture = await profilePictureService.SavePicture(model.ProfilePictureFile, data)
            };

            await userRepository.AddAsync(user);
        }

        public Task<VibeNetUser?> GetByIdentityIdAsync(string userIdentityId)
        {
            var user = userRepository.GetAllAttached()
                .Where(u => u.VibeNetUserId == userIdentityId)
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

            if(user == null) return null;

            var model = new VibeNetUserProfileViewModel
            {
                Id = user.Id,
                IdentityId = user.VibeNetUserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                HomeTown = user.HomeTown,
                Birthday = user.Birthday.ToString(),
                ProfilePicture = await profilePictureService.GetProfilePictureAsync(user.Id)
            };

            return model;
        }
    }
}
