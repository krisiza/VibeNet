using Microsoft.AspNetCore.Identity;

namespace VibeNet.Core.Contracts
{
    public interface IIdentityUserService
    {
        Task<bool> DeleteIdentityUserAsync(Guid id);
    }
}
