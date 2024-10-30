using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VibeNet.Data
{
    public class VibeNetDbContext : IdentityDbContext
    {
        public VibeNetDbContext(DbContextOptions<VibeNetDbContext> options)
            : base(options)
        {
        }
    }
}
