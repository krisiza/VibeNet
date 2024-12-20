﻿using VibeNet.Core.ViewModels;
using VibeNetInfrastucture.Data.Models;


namespace VibeNet.Core.Interfaces
{
    public interface IVibeNetService
    {
        Task<int> AddUserAsync(VibeNetUserFormViewModel model);

        Task<VibeNetUser?> GetByIdentityIdAsync(string userIdentityId);

        Task<VibeNetUser?> GetByIdAsync(int id);

        Task<bool> UpdateAsync(VibeNetUser item);

        Task<VibeNetUserProfileViewModel?> CreateVibeNetUserProfileViewModel(string userId);

        Task DeleteAsync(string userId);

        Task<VibeNetUserFormViewModel?> CreateFormUserViewModel(string userId);

        Task<(IEnumerable<VibeNetUserProfileViewModel> Users, int TotalCount)> FindUsers(string searchedTerm,string category, string userId, int pageNumber, int pageSize);
    }
}
