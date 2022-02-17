using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Data.Entities;
using System;

namespace SocialNetwork.Data.Configurations
{
    internal class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder
                .Property(l => l.CreatedAt)
                .HasDefaultValueSql("getdate()");
        }
    }
}
