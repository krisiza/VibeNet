using Microsoft.AspNetCore.Http;
using VibeNet.Core.ViewModels;

namespace VibeNet.Core.Contracts
{
    public interface IProfilePictureService
    {
        Task<ProfilePictureViewModel?> GetProfilePictureAsync(int id);
        Task SavePicture(IFormFile formFile, int userId, byte[] data);

    }
}
