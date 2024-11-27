using System.Diagnostics.CodeAnalysis;
using VibeNet.Core.ViewModels;

namespace Vibenet.Tests.CustomComparers
{
    public class VibeNetUserProfileViewModelComparer : IEqualityComparer<VibeNetUserProfileViewModel>
    {
        public bool Equals(VibeNetUserProfileViewModel x, VibeNetUserProfileViewModel y)
        {
            if (x == null || y == null)
                return false;

            return x.Id == y.Id &&
                   x.IdentityId == y.IdentityId &&
                   x.FirstName == y.FirstName &&
                   x.LastName == y.LastName &&
                   x.Gender == y.Gender &&
                   x.HomeTown == y.HomeTown &&
                   x.Birthday == y.Birthday &&
                   Equals(x.ProfilePicture, y.ProfilePicture);
        }

        public int GetHashCode(VibeNetUserProfileViewModel obj)
        {
            return HashCode.Combine(obj.Id, obj.IdentityId, obj.FirstName, obj.LastName, obj.Gender, obj.HomeTown, obj.Birthday, obj.ProfilePicture);
        }
    }
}
