using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VibeNet.Infrastucture.Data.Models;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Infrastucture.Data
{
    public class VibeNetDbContext : IdentityDbContext
    {
        public VibeNetDbContext(DbContextOptions<VibeNetDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<VibeNetUser> VibeNetUsers { get; set; }
        public virtual DbSet<Friendship> Friendships { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Friendshiprequest> Friendshiprequests { get; set; }
        public virtual DbSet<Post> Posts { get; set; }

        public virtual DbSet<ProfilePicture> ProfilePictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Friendshiprequest>()
                .HasOne(fr => fr.UserRecipient)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendshiprequest>()
              .HasOne(fr => fr.UserTransmitter)
              .WithMany()
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.FirstUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendship>()
              .HasOne(f => f.SecondUser)
              .WithMany()
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
