using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace VibeNet.Core.Utilities
{
    public static class VibeNetHepler
    {
        public static string GetDisplayName(this Enum value)
        {
            var type = value.GetType();
            var member = type.GetMember(value.ToString()).FirstOrDefault();
            var attribute = member?.GetCustomAttribute<DisplayAttribute>();
            return attribute?.Name ?? value.ToString();
        }
        public static async Task<byte[]> ConvertToBytesAsync(IFormFile? file)
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

