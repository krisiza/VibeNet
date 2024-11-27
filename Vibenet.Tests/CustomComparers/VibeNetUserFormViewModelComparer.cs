using VibeNet.Core.ViewModels;

namespace Vibenet.Tests.CustomComparers
{
    public class VibeNetUserFormViewModelComparer : IEqualityComparer<VibeNetUserFormViewModel>
    {
        public bool Equals(VibeNetUserFormViewModel x, VibeNetUserFormViewModel y)
        {
            if (x == null || y == null)
                return false;

            return x.Id == y.Id &&
                   x.FirstName == y.FirstName &&
                   x.LastName == y.LastName &&
                   x.Gender == y.Gender &&
                   x.HomeTown == y.HomeTown &&
                   x.Birthday == y.Birthday &&
                   x.ProfilePictureFile == x.ProfilePictureFile;
        }

        public int GetHashCode(VibeNetUserFormViewModel obj)
        {
            return HashCode.Combine(obj.Id, obj.FirstName, obj.LastName, obj.Gender, obj.HomeTown, obj.Birthday, obj.ProfilePictureFile);
        }
    }
}
