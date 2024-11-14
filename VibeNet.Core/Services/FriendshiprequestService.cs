using Microsoft.EntityFrameworkCore;
using VibeNet.Core.Contracts;
using VibeNet.Core.Interfaces;
using VibeNet.Core.ViewModels;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Core.Services
{
    public class FriendshiprequestService : IFriendshiprequestService
    {
        private readonly IRepository<Friendshiprequest, object> friendshiprequestRepository;
        private readonly IVibeNetService vibeNetService;

        public FriendshiprequestService(IRepository<Friendshiprequest, object> friendshiprequestRepositora,
            IVibeNetService vibeNetService)
        {
            this.friendshiprequestRepository = friendshiprequestRepositora;
            this.vibeNetService = vibeNetService;
        }

        public async Task<IEnumerable<VibeNetUserProfileViewModel>?> GetFriendrequets(string userId)
        {
            IList<VibeNetUserProfileViewModel>? models = new List<VibeNetUserProfileViewModel>();

            var entityList = await friendshiprequestRepository.GetAllAttached()
                .Where(fr => fr.UserRecipientId == userId)
                .ToListAsync();

            if (entityList.Count == 0)
                return null;

            foreach (var entity in entityList)
            {
                var model = await vibeNetService.CreateVibeNetUserProfileViewModel(entity.UserTransmitterId);
                models.Add(model);
            }

            return models;
        }

        public async Task<bool> FindByIdAsync(string userRecipient, string userTransmitter)
        {
            var entityList = await friendshiprequestRepository.GetAllAttached()
                .Where(fr => fr.UserTransmitterId == userTransmitter && fr.UserRecipientId == userRecipient)
                .ToListAsync();

            if (entityList.Count > 0)
                return false;

            return true;
        }

        public async Task SendRequestAsync(string userRecipient, string userTransmitter)
        {
            Friendshiprequest entity = new Friendshiprequest() 
            {
                UserRecipientId = userRecipient,
                UserTransmitterId = userTransmitter
            };

            await friendshiprequestRepository.AddAsync(entity);
        }
    }
}
