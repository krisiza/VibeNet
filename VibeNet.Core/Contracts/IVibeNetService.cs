using System.Security.Cryptography;
using VibeNet.Core.ViewModels;
using VibeNetInfrastucture.Data.Models;
using static VibeNetInfrastucture.Validations.ValidationConstants;

namespace VibeNet.Core.Interfaces
{
    public interface IVibeNetService
    {
        Task AddUserAsync(VibeNetUserRegisterViewModel model);

        Task<VibeNetUser?> GetByIdentityIdAsync(string userIdentityId);

        Task<VibeNetUser?> GetByIdAsync(int id);

        Task<bool> UpdateAsync(VibeNetUser item);

        Task<VibeNetUserProfileViewModel> CreateVibeNetUserProfileViewModel(string userId);
    }
}
