using VibeNet.Core.ViewModels;

namespace VibeNet.Core.Interfaces
{
    public interface IVibeNetService
    {
        Task AddUserAsync(VibeNetUserRegisterViewModel model);
    }
}
