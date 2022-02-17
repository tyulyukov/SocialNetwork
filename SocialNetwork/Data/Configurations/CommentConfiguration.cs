using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Data.Entities;
using System;

namespace SocialNetwork.Data.Configurations
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("getdate()");
        }
    }
}
