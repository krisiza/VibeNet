using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VibeNetInfrastucture.Data.Models;

namespace VibeNet.Infrastucture.Data.SeedDb
{
    internal class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            var data = new SeedData();

            builder.HasData(new Post[] { data.Post });
        }
    }
}
