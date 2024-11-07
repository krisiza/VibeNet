using Microsoft.AspNetCore.Identity;
using VibeNet.Core.Contracts;
using VibeNet.Infrastucture.Repository.Contracts;

namespace VibeNet.Core.Services
{
    public class IdentityUserService : IIdentityUserService
    {
        private readonly IRepository<IdentityUser, Guid> identityUserRepository;

        public IdentityUserService(IRepository<IdentityUser, Guid> identityUserRepository)
        {
            this.identityUserRepository = identityUserRepository;
        }

        public async Task<bool> DeleteIdentityUserAsync(Guid id)
            => await identityUserRepository.DeleteAsync(id);
    }
}
