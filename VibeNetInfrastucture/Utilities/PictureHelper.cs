using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace VibeNet.Infrastucture.Utilities
{
    public static class PictureHelper
    {
        public static async Task<byte[]> ConvertToBytesAsync(string pictureName)
        {
            string defaultPicturePath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.Parent!.FullName, "ProfilePictures", pictureName);
            return await File.ReadAllBytesAsync(defaultPicturePath);
        }

        public static async Task<IFormFile?> CreateFormFileFromByteArray(string pictureName)
        {
            string defaultPicturePath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.Parent!.FullName, "ProfilePictures", pictureName);

            if (!File.Exists(defaultPicturePath))
            {
                throw new FileNotFoundException($"The file at {defaultPicturePath} was not found.");
            }

            byte[] fileData = await File.ReadAllBytesAsync(defaultPicturePath);

            var fileName = Path.GetFileName(defaultPicturePath);
            var contentType = "jpg";

            IFormFile formFile = null;
            using (var stream = File.OpenRead(defaultPicturePath))
            {
                formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "jpg"
                };
            }

            return formFile;
        }
    }
}
