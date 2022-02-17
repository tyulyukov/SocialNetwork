using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Data.Entities;
using System;

namespace SocialNetwork.Data.Configurations
{
    internal class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder
                .Property(i => i.CreatedAt)
                .HasDefaultValueSql("getdate()");

            builder
                .HasOne(i => i.Post)
                .WithMany(p => p.Attachments);
        }
    }
}
