using Microsoft.EntityFrameworkCore;
using VibeNet.Core.Contracts;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Core.Services
{
    public class FriendshiprequestService : IFriendshiprequestService
    {
        private readonly IRepository<Friendshiprequest, object> friendshiprequestRepository;

        public FriendshiprequestService(IRepository<Friendshiprequest, object> friendshiprequestRepositora)
        {
            this.friendshiprequestRepository = friendshiprequestRepositora;
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
                SendOn = DateTime.Now,
                UserRecipientId = userRecipient,
                UserTransmitterId = userTransmitter
            };

            await friendshiprequestRepository.AddAsync(entity);
        }
    }
}
