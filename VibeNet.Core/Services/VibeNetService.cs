using VibeNet.Core.Interfaces;
using VibeNet.Infrastucture.Repository.Contracts;
using VibeNet.Core.ViewModels;
using VibeNetInfrastucture.Data.Models;
using VibeNet.Core.Mapping;
using AutoMapper;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using AutoMapper.Internal.Mappers;
using Microsoft.EntityFrameworkCore;

namespace VibeNet.Core.Services
{
    public class VibeNetService : IVibeNetService
    {
        private readonly IRepository<VibeNetUser, int> userRepository;
        private readonly IMapper mapper;

        public VibeNetService(IRepository<VibeNetUser, int> userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task AddUserAsync(VibeNetUserRegisterViewModel model)
        {
            VibeNetUser user = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                HomeTown = model.HomeTown,
                Birthday = Convert.ToDateTime(model.Birthday, CultureInfo.InvariantCulture),
                CreatedOn = Convert.ToDateTime(model.CreatedOn, CultureInfo.InvariantCulture),
                Gender = model.Gender,
                IsDeleted = model.IsDeleted,
                VibeNetUserId = model.Id.ToString(),
            };

            await userRepository.AddAsync(user);
        }

        public Task<VibeNetUser?> GetByIdentityIdAsync(string userIdentityId)
        {
            var user = userRepository.GetAllAttached()
                .Where(u => u.VibeNetUserId == userIdentityId)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<VibeNetUser?> GetByIdAsync(int id)
        {
            var user = await userRepository.GetAllAttached()
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> UpdateAsync(VibeNetUser item)
            =>await userRepository.UpdateAsync(item);
    }
}
