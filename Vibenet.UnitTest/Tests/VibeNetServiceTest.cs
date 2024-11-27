using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;

using VibeNet.Core.ViewModels;
using VibeNetInfrastucture.Data.Models.Enums;
using Xunit;

namespace Vibenet.UnitTest.Tests
{
    internal class VibeNetServiceTest
    {
      private List<VibeNetUserProfileViewModel> userModels;

        [SetUp]
        public void Setup()
        {
            userModels = new List<VibeNetUserProfileViewModel>() 
            {

            };
        }
    }
}
