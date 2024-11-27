using Microsoft.AspNetCore.Http;
using VibeNet.Core.ViewModels;
using VibeNet.Infrastucture.Data.Models;

namespace VibeNet.Core.Contracts
{
    public interface IProfilePictureService
    {
        Task<ProfilePictureViewModel?> GetProfilePictureAsync(int? id);
        Task<ProfilePicture> SavePicture(IFormFile formFile, byte[] data);
        void Delete(int pictureId);
    }
}
