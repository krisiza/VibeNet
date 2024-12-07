using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace VibeNet.Core.Utilities
{
    public class PictureHelper : IPictureHelper
    {
        public async Task<byte[]> ConvertToBytesAsync(IFormFile? file)
        {
            string defaultPicturePath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName, "wwwroot", "my-icons", "profile-avatar.jpg");

            if (file == null || file.Length == 0)
                return await File.ReadAllBytesAsync(defaultPicturePath);

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}

