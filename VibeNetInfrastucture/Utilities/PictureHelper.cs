using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeNet.Infrastucture.Utilities
{
    public static class PictureHelper
    {
        public static async Task<byte[]> ConvertToBytesAsync(string pictureName)
        {
            string defaultPicturePath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.Parent!.FullName, "ProfilePictures", pictureName);
                return await File.ReadAllBytesAsync(defaultPicturePath);
        }
    }
}
