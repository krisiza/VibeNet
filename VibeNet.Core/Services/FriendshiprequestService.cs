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
        private readonly IFriendshipService friendshipService;

        public FriendshiprequestService(IRepository<Friendshiprequest, object> friendshiprequestRepositora,
            IVibeNetService vibeNetService,IFriendshipService friendshipService)
        {
            this.friendshiprequestRepository = friendshiprequestRepositora;
            this.vibeNetService = vibeNetService;
            this.friendshipService = friendshipService;
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
                return true;

            return false;
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

        public async Task DeleteRequest(string transitterId, string recipientId)
        {
            var entity = await friendshiprequestRepository.GetAllAttached()
                .Where(fr => fr.UserTransmitterId == transitterId && fr.UserRecipientId == recipientId)
                .FirstOrDefaultAsync();

            if (entity == null) return;
            await friendshiprequestRepository.DeleteEntityAsync(entity);

        }

        public async Task AcceptRequest(string transitterId, string recipientId)
        {
            var entity = await friendshiprequestRepository.GetAllAttached()
                .Where(fr => fr.UserTransmitterId == transitterId && fr.UserRecipientId == recipientId)
                .FirstOrDefaultAsync();

            if (entity == null) return;
            await friendshiprequestRepository.DeleteEntityAsync(entity);

            await friendshipService.AddFriendShipAsync(entity);
        }
    }
}
