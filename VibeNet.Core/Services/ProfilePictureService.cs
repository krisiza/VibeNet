using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VibeNet.Core.Contracts;
using VibeNet.Core.ViewModels;
using VibeNet.Infrastucture.Data.Models;
using VibeNet.Infrastucture.Repository.Contracts;

namespace VibeNet.Core.Services
{
    public class ProfilePictureService : IProfilePictureService
    {
        private readonly IRepository<ProfilePicture, int> profilePictureRepository;
        public ProfilePictureService(IRepository<ProfilePicture, int> profilePictureRepository)
        {
            this.profilePictureRepository = profilePictureRepository;
        }
        public async Task<ProfilePictureViewModel?> GetProfilePictureAsync(int userid)
        {
            var entity = await profilePictureRepository.GetAllAttached()
                  .Include(p => p.VibeNetUser)
                  .Where(p => p.VibeNetUser.Id == userid)
                  .FirstOrDefaultAsync();

            if (entity == null) return null;

            ProfilePictureViewModel viewModel = new ProfilePictureViewModel()
            {
                Id = entity.Id,
                ContentType = entity.ContentType,
                Data = entity.Data,
                Name = entity.Name,
                OwnerId = entity.VibeNetUser.Id,
            };

            return viewModel;
        }

        public async Task SavePicture(IFormFile formFile, int userId, byte[] data)
        {
            ProfilePicture entity = new()
            {
                Name = formFile.Name,
                OwnerId = userId,
                ContentType = formFile.ContentType,
                Data = data
            };

            await profilePictureRepository.AddAsync(entity);
        }
    }
}
