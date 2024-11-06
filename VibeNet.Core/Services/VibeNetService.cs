using VibeNet.Core.Interfaces;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNet.Core.ViewModels;
using VibeNetInfrastucture.Data.Models;
using VibeNet.Core.Mapping;

namespace VibeNet.Core.Services
{
    public class VibeNetService : IVibeNetService
    {
        private IRepository<VibeNetUser, int> userRepository;

        public VibeNetService(IRepository<VibeNetUser, int> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task AddUserAsync(VibeNetUserRegisterViewModel model)
        {
            VibeNetUser user = new VibeNetUser();
            AutoMapperConfig.MapperInstance.Map(user, model);
            await userRepository.AddAsync(user);
        }
    }
}
