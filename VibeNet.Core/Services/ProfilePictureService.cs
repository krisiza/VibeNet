using Microsoft.AspNetCore.Http;
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
        public async Task<ProfilePictureViewModel?> GetProfilePictureAsync(int pictureId)
        {
            var entity = await profilePictureRepository.GetAllAttached()
                  .Where(p => p.Id == pictureId)
                  .FirstOrDefaultAsync();

            if (entity == null) return null;

            ProfilePictureViewModel viewModel = new ProfilePictureViewModel()
            {
                Id = entity.Id,
                ContentType = entity.ContentType,
                Data = entity.Data,
                Name = entity.Name,
            };

            return viewModel;
        }

        public async Task<ProfilePicture> SavePicture(IFormFile? formFile, byte[] data)
        {
            ProfilePicture entity = new();

            if (formFile == null)
            {
                entity.Name = "profile-avatar";
                entity.ContentType = ".jpg";
                entity.Data = data;
            }
            else
            {
                entity.Name = formFile.Name;
                entity.ContentType = formFile.ContentType;
                entity.Data = data;
            };

            await profilePictureRepository.AddAsync(entity);

            return entity;
        }
    }
}
