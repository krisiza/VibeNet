using Microsoft.AspNetCore.Http;

namespace VibeNet.Core.Utilities
{
    public interface IPictureHelper
    {
        Task<byte[]> ConvertToBytesAsync(IFormFile? file);
    }
}
