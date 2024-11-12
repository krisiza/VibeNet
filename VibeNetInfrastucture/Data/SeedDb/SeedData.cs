using Microsoft.AspNetCore.Identity;
using VibeNet.Infrastucture.Data.Models;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Infrastucture.Data.SeedDb
{
    internal class SeedData
    {
        public IdentityUser User { get; set; }
        public Post Post { get; set; }
        public Comment Comment { get; set; }
        public ProfilePicture ProfilePicture { get; set; }

        //Add all

        public SeedData()
        {
            SeedUsers();
            SeedPosts();
            SeedComments();
            SeedRelations();    
        }

        public void SeedUsers()
        {

        }

        public void SeedComments() 
        {

        }

        public void SeedPosts()
        {

        }

        public void SeedRelations()
        {

        }
    }
}
